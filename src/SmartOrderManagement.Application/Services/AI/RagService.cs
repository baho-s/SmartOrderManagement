using SmartOrderManagement.Application.Interfaces.AI;
using SmartOrderManagement.Application.Interfaces.HuggingFace;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Services.AI
{
    public class RagService : IRagService
    {
        private readonly IProductRepository _productRepository;
        private readonly IHuggingFaceService _huggingFaceService;

        public RagService(IProductRepository productRepository, IHuggingFaceService huggingFaceService)
        {
            _productRepository = productRepository;
            _huggingFaceService = huggingFaceService;
        }

        public async Task<string> GetAugmentedPromptAsync(string userQuestion)
        {
            var products = await _productRepository.GetProductsAsyncForRAG();//List<Product> döndürüyor

            var documents=new List<string>();

            foreach (var product in products)
            {
                var doc = "Id:" + product.ProductId.ToString() + $" Name: {product.ProductName}, Price: {product.ProductPrice}";
                documents.Add(doc);
            }

            var relevantDocs = RetrieveRelevantDocs(userQuestion, documents, topK: 2);

            var augmentedPrompt = BuildAugmentedPrompt(userQuestion, relevantDocs);
           var response= await _huggingFaceService.CallLLMWithRAG(augmentedPrompt, "api_key");
            return response;
        }

        static double CalculateSimilarity(string text1, string text2)
        {
            // Her iki metni de küçük harfe çevir ve kelime kütlesi oluştur
            // Where(w => w.Length > 2) → 2 harften kısa olanları (ve, de gibi) atla
            var words1 = text1.ToLower().Split(' ').Where(w => w.Length > 2).ToHashSet();
            var words2 = text2.ToLower().Split(' ').Where(w => w.Length > 2).ToHashSet();

            // Eğer kelime yok ise benzerlik 0 (boş metin)
            if (words1.Count == 0 || words2.Count == 0) return 0;

            // Intersection (kesişim) = Her iki metin içinde de var olan kelimeler
            // Örnek: "İstanbul" ve "İstanbul hakkında" → "İstanbul" ortak
            var intersection = words1.Intersect(words2).Count();

            // Union (birleşim) = Tüm benzersiz kelimeler (bir kere say)
            // Örnek: "İstanbul hakkında" + "Türkiye İstanbul" → "İstanbul", "hakkında", "Türkiye"
            var union = words1.Union(words2).Count();

            // Jaccard Similarity = Ortak Kelimeler / Tüm Benzersiz Kelimeler
            // Bu yüzde ne kadar benzer olduğunu gösterir
            return (double)intersection / union;
        }



        static List<string> RetrieveRelevantDocs(string question, List<string> products, int topK = 2)
        {
            // ADIM 1: Her belgeyi soru ile karşılaştır ve benzerlik skoru hesapla
            var value = products[0];
            var scored = products
                .Select(doc => new
                {
                    Document = doc,
                    Score = CalculateSimilarity(question, doc)  // 0-1 arası skor
                })
                .OrderByDescending(x => x.Score)  // En yüksek skor önce (azalan sıra)
                .Take(topK)                         // En iyi topK belgeyi al (2. belgeyi)
                .Select(x => x.Document)           // Sadece belge metni al, skoru atla
                .ToList();

            return scored;
        }

        // ========== PROMPT HAZIRLAMA FONKSİYONU ==========
        /// <summary>
        /// Bulunan belgeleri sorunun başına ekleyerek zenginleştirilmiş prompt oluşturur
        /// Bu sayede LLM doğru bilgiye dayanarak daha iyi cevap verir
        /// 
        /// Klasik prompt: "İstanbul hakkında bilgi ver"
        /// RAG prompt:   "Şu belgeler var: [belgeler]. Bunu kullanarak cevap ver"
        /// </summary>
        static string BuildAugmentedPrompt(string question, List<string> relevantDocs)
        {
            // Belgeleri numaralandırılmış formatta birleştir
            // "1. Ankara...\n2. Istanbul..." şeklinde
            string context = string.Join("\n",
                relevantDocs.Select((doc, i) => $"{i + 1}. {doc}"));

            // Prompt şablonu oluştur
            // @"" = verbatim string (escape karakterleri içermez, çok satırlı yazılabilir)
            string augmentedPrompt = $@"Şu belgeler verilmiştir:
            {context}

            Bu bilgilere dayanarak ürünlerin yalnızca Id'sini yaz.
            {question}";

            return augmentedPrompt;
        }
    }
}
