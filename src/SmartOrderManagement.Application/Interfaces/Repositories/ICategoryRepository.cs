using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        // Bu metod veritabanından veri getirecek.
        // Veritabanı işlemi zaman alabileceği için async çalışmalı.
        // Bu yüzden dönüş tipi Category değil, Task<Category?> olur.
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category entity);
        Task UpdateAsync(Category entity);

        // Bu metod silme işlemini yapacak.
        // Silme işleminin gerçek kısmı SaveChangesAsync ile DB'ye gideceği için
        // bu metod da async olmalı.
        // Geriye veri döndürmüyorsak Task kullanırız.

        //DeleteAsync sadece gelen entity'i siler, alt kategorileri silmez.Alt kategorisi yoksa bu methodu kullanıcaz.
        Task DeleteAsync(Category entity);
        // DeleteRecursiveAsync ise gelen entity'i ve alt kategorilerini siler.//Alt kategorisi varsa ve silmek istiyorsak bu methodu kullanıcaz.
        Task DeleteRecursiveAsync(Category category);

        Task TransferSubCategoriesAsync(Category category, int? newParentId);

        Task<List<Category>> GetCategoriesAsync();


        Task<List<Category>> GetAllAsync(GetCategoryQueryDto queryDto);

        //Verilen kategori adının veritabanında zaten var olup olmadığını kontrol eder.
        //Veritabanı işlemleri async yapılmalı.
        Task<bool> IsCatergoyNameExistsAsync(string categoryName,int id=0);

        // Update sırasında aynı isim başka kayıtta var mı kontrolü
        Task<bool> IsCategoryNameExistsForUpdateAsync(int categoryId, string categoryName);

        // Yeni metod: Hiyerarşik yapı için
        Task<List<Category>> GetAllCategoriesForTreeAsync();

        Task<Category?> GetCategoryWithSubAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task SaveChangesAsyncR();


    }
}
