using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category entity);
        void DeleteAsync(Category entity);
        Task<List<Category>> GetCategoriesAsync();
               
        //Verilen kategori adının veritabanında zaten var olup olmadığını kontrol eder.
        Task<bool> IsCatergoyNameExistsAsync(string categoryName,int id=0);

        // Update sırasında aynı isim başka kayıtta var mı kontrolü
        Task<bool> IsCategoryNameExistsForUpdateAsync(int categoryId, string categoryName);

        // Yeni metod: Hiyerarşik yapı için
        Task<List<Category>> GetAllCategoriesForTreeAsync();

        //Silinen kategorinin alt kategorilerini yeni üst kategoriye taşıma
        Task<Category?> GetCategoryWithSubAsync(int id);
        
        Task<bool> ExistsAsync(int id);

    }
}
