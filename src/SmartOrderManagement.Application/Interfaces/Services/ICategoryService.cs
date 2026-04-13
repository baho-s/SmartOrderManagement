using SmartOrderManagement.Application.Common.ApiResponse;
using SmartOrderManagement.Application.DTOs.CategoryDtos;

namespace SmartOrderManagement.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        // Service dışarıya DTO döner.
        // Çünkü API'ye entity vermek istemiyoruz.
        Task<List<CategoryListDto>> GetCategoriesAsync();

        Task<ApiResponse<CategoryByIdDto>> GetByIdAsync(int id);

        //Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);

        //Task UpdateCategoryAsync(int id,UpdateCategoryDto updateCategoryDto);

        Task<List<CategoryTreeDto>> GetCategoryTreeAsync();


        // Controller bu metodu çağıracak.
        // İçeride repository'nin async metodlarını kullanacağımız için
        // service metodu da async akışa uygun olmalı.
       // Task DeleteCategoryAsync(int id,bool AltKategoriSil,int? newParentId);

        Task<List<CategoryListDto>> GetAllAsync(GetCategoryQueryDto queryDto);
        // Tek tek page, pageSize göndermek yerine
        // tek bir DTO ile tüm parametreleri taşıyoruz
        // Bu yapı gerçek projelerde kullanılır (scalable yapı)
    }
}
