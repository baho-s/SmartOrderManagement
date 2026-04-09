using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SmartOrderManagement.Application.Common.ApiResponse;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators;
using SmartOrderManagement.Application.Validators.CategoryValidators;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICreateCategoryValidator _createCategoryValidator;
        private readonly IUpdateCategoryValidator _updateCategoryValidator;

        //Repository dışarıdan gelir(DI)
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ICreateCategoryValidator createCategoryValidator, IUpdateCategoryValidator updateCategoryValidator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createCategoryValidator = createCategoryValidator;
            _updateCategoryValidator = updateCategoryValidator;
        }

        //Güncellendi
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {

            var result = await _createCategoryValidator.ValidateAsync(createCategoryDto);
            //Şartlar sağlandımı kontrolü yapıyoruz. EĞER sağlanırsa ApiResponse ile geri dönüşleri standart hale getirmiştik.
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                // Hata mesajlarını listeye çevir
                // ["Kategori adı boş geçilemez.", "Kategori adı en az 2 karakter olmalıdır."]

                throw new ValidationMyException(errors);//Middleware yakalayacak.
            }


            // Kullanıcının gönderdiği kategori adı veritabanında zaten var mı diye kontrol ediyoruz
            bool isCategoryNameExists = await _categoryRepository.IsCatergoyNameExistsAsync(createCategoryDto.CategoryName);
            if (isCategoryNameExists)
            {
                throw new BusinessRuleException($"'{createCategoryDto.CategoryName}' adında bir kategori zaten mevcut");//Middleware yakalyacak.

                // Eğer aynı isimde kategori varsa
                // iş kuralı ihlali oluşur
                //
                // Bu yüzden özel exception fırlatıyoruz
            }

            // Eğer parent category id gönderilmişse, bu id'ye sahip bir kategori var mı diye kontrol ediyoruz
            if (createCategoryDto.ParentCategoryId.HasValue && 
                createCategoryDto.ParentCategoryId != null)
            {
                var parentExists=await _categoryRepository.GetByIdAsync(createCategoryDto.ParentCategoryId.Value);
                if (parentExists == null)
                {
                    throw new BusinessRuleException($"İd'si: {createCategoryDto.ParentCategoryId} olan parent" +
                        $" kategori bulunamadı");
                }
            }

            if (createCategoryDto.ParentCategoryId == 0)
            {
                createCategoryDto.ParentCategoryId = null;
            }

            //DTO->Entity dönüşümü
            var category = _mapper.Map<Category>(createCategoryDto);

            //Repository üzerinden veritabanına kaydet
            await _categoryRepository.AddAsync(category);

            return _mapper.Map<CategoryDto>(category);
            //Id ile dönüş yapıyoruz çünkü kaydetme işlemi sırasında entity'nin Id'si oluşur ve biz bunu geri dönmek isteriz.
        }

        public async Task DeleteCategoryAsync(int id, bool AltKategoriSil, int? newParentId)
        {
            var entity = await _categoryRepository.GetCategoryWithSubAsync(id);

            if (entity == null)
            {
                throw new BusinessRuleException($"İd'si: {id} olan Kategori bulunamadı");
            }

            //AltKategori yoksa direkt sil.
            if (!entity.SubCategories.Any())
            {
                await _categoryRepository.DeleteAsync(entity);
                return;
            }

            //Alt Kategorilerde silinsin.
            if (AltKategoriSil)
            {
                await _categoryRepository.DeleteRecursiveAsync(entity);
                return;
            }

            //Alt Kategoriler silinmeyecekse yeni parent zorunlu.
            if (!newParentId.HasValue)
            {
                throw new BusinessRuleException("Alt kategoriler silinmeyecekse yeni parent category id'si zorunludur.");
            }

            // Kendisine taşıma engeli
            if (newParentId.Value == id)
            {
                throw new BusinessRuleException("Bir kategori kendi alt kategorilerinin yeni üst kategorisi olamaz.");
            }

            var newParent = await _categoryRepository.GetByIdAsync(newParentId.Value);
            if (newParent == null)
            {
                throw new BusinessRuleException($"İd'si: {newParentId.Value} olan yeni parent kategori bulunamadı");
            }

            // Önce çocukları taşı
            await _categoryRepository.TransferSubCategoriesAsync(entity, newParentId);


            // Sonra ana kategoriyi sil
            await _categoryRepository.DeleteAsync(entity);
        }



        public async Task<List<CategoryListDto>> GetCategoriesAsync()
        {
            // Repository'den entity listesi alıyoruz.
            // Bu işlem async olduğu için await kullanıyoruz.
            var values = await _categoryRepository.GetCategoriesAsync();

            // Entity listesini DTO listesine çeviriyoruz.
            // Çünkü dışarıya entity vermek istemiyoruz.
            //var categoryListDtos = values.Select(x => new CategoryListDto
            //{
            //    // Burada mapping yapıyoruz (entity → dto)
            //    CategoryId = x.CategoryId,
            //    CategoryName = x.CategoryName
            //}).ToList();

            var categoryListDtos = _mapper.Map<List<CategoryListDto>>(values);

            // DTO listesini geri döndürüyoruz.
            return categoryListDtos;
        }

        public async Task<ApiResponse<CategoryByIdDto>> GetByIdAsync(int id)
        {
            var value = await _categoryRepository.GetByIdAsync(id);
            if (value == null)
            {
                throw new BusinessRuleException($"İd'si: {id} olan Kategori bulunamadı");
            }

            //Tek veri varsa select kullanmıyoruz.Direkt Mapleme yapıyoruz.          
            var categoryByIdDto = new CategoryByIdDto
            {
                CategoryId = value.CategoryId,
                CategoryName = value.CategoryName
            };

            return ApiResponse<CategoryByIdDto>.Succes(categoryByIdDto);
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {

            // ID uyuşmazlığı kontrolü (çok önemli)
            if (id != updateCategoryDto.CategoryId)
            {
                throw new BusinessRuleException("Id uyuşmuyor.");
            }

            var result=_updateCategoryValidator.ValidateAsync(updateCategoryDto);
            if (!result.Result.IsValid)
            {
                var errors = result.Result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationMyException(errors);
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new BusinessRuleException($"İd'si: {id} olan Kategori bulunamadı");
            }

            if (category.CategoryName.ToLower() != updateCategoryDto.CategoryName.ToLower())
            {

                bool isCategoryExists = await _categoryRepository.IsCategoryNameExistsForUpdateAsync(id, updateCategoryDto.CategoryName);
                //Bu Method aynı isimde kategori varmı diye sorar.

                if (isCategoryExists)
                {
                    throw new BusinessRuleException("Kategori zaten mevcut,olmayan bir kategori adı giriniz.");
                    // Aynı isimde kategori varsa genel Exception yerine
                    // BusinessRuleException fırlatıyoruz
                    //
                    // Böylece bu hatanın bir sistem hatası değil,
                    // iş kuralı ihlali olduğu anlaşılır
                }

            }

            //Burayı eğer veriler hiç değişmeden update'e tıklanırsa extra güncelleme veritabanına boş sorgu olarak gitmemesi için konuldu.
            // Değişiklik var mı kontrol et
            bool hasChanges =
                category.CategoryName != updateCategoryDto.CategoryName ||
                category.CategoryDescription != updateCategoryDto.CategoryDescription ||
                category.ImageUrl != updateCategoryDto.ImageUrl ||
                category.ParentCategoryId != updateCategoryDto.ParentCategoryId;

            if (!hasChanges)
            {
                // Hiçbir şey değişmediyse gereksiz UPDATE yapma
                return;
            }
            // Tüm alanları güncelle, 
            _mapper.Map(updateCategoryDto, category);



            await _categoryRepository.UpdateAsync(category);

        }

        public async Task<List<CategoryListDto>> GetAllAsync(GetCategoryQueryDto queryDto)
        {
            var values = await _categoryRepository.GetAllAsync(queryDto);
            // Repository'den filtrelenmiş ve sayfalanmış verileri aldık

            return values.Select(x => new CategoryListDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            }).ToList();
            // Entity → DTO dönüşümü yaptık
            // API dışarıya direkt entity vermez (çok önemli kural)
        }

        public async Task<List<CategoryTreeDto>> GetCategoryTreeAsync()
        {

            var categories = await _categoryRepository.GetAllCategoriesForTreeAsync();
            // Repository'den tüm kategorileri aldık. Bu kategoriler düz bir liste halinde olacak.

            var rootCategories = categories.Where(c => c.ParentCategoryId == null).ToList();
            // Kök kategorilerden başlayarak ağaç yapısını oluştur

            var tree = new List<CategoryTreeDto>();

            foreach (var root in rootCategories)
            {
                var node = BuildCategoryTree(root, categories);// Kök kategoriden başlayarak alt kategorileri de içeren bir ağaç yapısı oluşturuyoruz
                tree.Add(node);// Oluşturulan ağaç yapısını sonuç listesine ekliyoruz
            }

            return tree;
        }

        private CategoryTreeDto BuildCategoryTree(Category category, List<Category> allCategories)
        {
            var dto=_mapper.Map<CategoryTreeDto>(category);

            // Verilen kategori için alt kategorileri bul
            var children = allCategories
                .Where(c => c.ParentCategoryId == category.CategoryId)
                .ToList();

            // Alt kategoriler varsa, her biri için aynı işlemi yaparak ağaç yapısını oluştur
            dto.SubCategories = new List<CategoryTreeDto>();

            foreach (var child in children)
            {
                dto.SubCategories.Add(BuildCategoryTree(child, allCategories));
            }

            return dto;
        }
    }
}
