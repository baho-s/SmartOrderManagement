using SmartOrderManagement.Application.Interfaces.HuggingFace;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartOrderManagement.Infrastructure.Services
{
    public class HuggingFaceService : IHuggingFaceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUrl = "https://router.huggingface.co/v1/chat/completions";

        public HuggingFaceService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> CallLLMWithRAG(string augmentedPrompt, string token)
        {
            var payload = new
            {
                model = "meta-llama/Llama-3.1-8B-Instruct:novita",
                messages = new[]
                {
                new { role = "user", content = augmentedPrompt }
            }
            };

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(ApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    // Burada loglama yapabilirsin
                    return $"API Hatası: {response.StatusCode}, Detay: {error}";
                }

                var responseText = await response.Content.ReadAsStringAsync();

                // System.Text.Json ile hızlıca parse işlemi
                using var doc = JsonDocument.Parse(responseText);
                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "Cevap boş döndü.";
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                return $"Bağlantı hatası: {ex.Message}";
            }
        }
    }
}
