using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        //Alt Kategorisi yoksa bunu kullanarak silme işlemi yapabiliriz.
        public async Task DeleteAsync(Category entity)
        {
            entity.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        // Recursive delete: Bir kategoriyi silerken alt kategorilerini de silmek istiyoruz.
        public async Task DeleteRecursiveAsync(Category category)
        {
            await _context.Entry(category)
                .Collection(c => c.SubCategories)
                .LoadAsync();

            category.IsDeleted = true;

            foreach (var subCategory in category.SubCategories)
            {

                await DeleteRecursiveInternalAsync(subCategory);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync(GetCategoryQueryDto queryDto)
        {
            var query = _context.Categories.AsQueryable();
            // IQueryable oluşturduk
            // Bu sayede üzerine filtre, sıralama vs. ekleyebiliriz
            // Henüz DB'ye gitmez (çok önemli)

            if (!string.IsNullOrWhiteSpace(queryDto.Search))
            {
                query = query.Where(x => x.CategoryName.Contains(queryDto.Search));
                // Eğer kullanıcı search parametresi gönderdiyse filtre uygula
                //
                // string.IsNullOrWhiteSpace neyi kontrol eder?
                // Search null mı, boş mu, sadece boşluk mu diye bakar
                //
                // Eğer search doluysa:
                // CategoryName içinde bu ifade geçen kayıtları getir
                //
                // Örnek:
                // search = "elek"
                // "Elektronik" eşleşebilir
                // "Ev Elektroniği" eşleşebilir
            }

            if (!string.IsNullOrWhiteSpace(queryDto.SortDirection))
            {
                if (!string.IsNullOrWhiteSpace(queryDto.SortDirection) &&
                    !string.IsNullOrWhiteSpace(queryDto.SortBy))
                {
                    if (queryDto.SortBy.ToLower() == "name")
                    {
                        //Eğer sıralama alanı "name" ise
                        //CategoryName alanına göre sıralama yapacağız.

                        query = queryDto.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.CategoryName) :
                            query.OrderBy(x => x.CategoryName);
                        // Eğer sortDirection = desc ise Z'den A'ya sıralar
                        // Değilse varsayılan olarak A'dan Z'ye sıralar


                        // ? operatörü null güvenliği sağlar
                        // SortDirection null ise hata vermesin diye kullandık
                    }
                    else if (queryDto.SortBy.ToLower() == "id")
                    {
                        //Eğer sıralama alanı "id" ise
                        //CategoryId alanına göre sıralama yapacağız.
                        query = queryDto.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.CategoryId)
                            : query.OrderBy(x => x.CategoryId);
                        // Eğer sortDirection = desc ise büyük id'den küçüğe sıralar
                        // Değilse küçük id'den büyüğe sıralar

                        // ? operatörü null güvenliği sağlar
                        // SortDirection null ise hata vermesin diye kullandık
                    }
                    else
                    {
                        query = query.OrderBy(x => x.CategoryId);
                        // Kullanıcı hiç sortBy göndermediyse
                        // yine varsayılan bir sıralama veriyoruz
                        // Bu önemlidir çünkü tutarlı sonuç döner
                    }
                }
            }

            if (queryDto.Page < 1)
            {
                queryDto.Page = 1;
                // Eğer kullanıcı page=0 veya negatif gönderirse
                // sistemi bozmaması için 1'e çektik
            }
            if (queryDto.PageSize < 1)
            {
                queryDto.PageSize = 10;
                // Eğer kullanıcı pageSize=0 veya negatif gönderirse
                // default değere çektik
            }

            // * Pagination işlemi
            int skipCount = (queryDto.Page - 1) * queryDto.PageSize;
            // Kaç kayıt atlanacak?
            // Örnek:
            // page=1 → skip=0
            // page=2 → skip=10
            // page=3 → skip=20

            query = query.Skip(skipCount).Take(queryDto.PageSize);
            // Skip → belirtilen kadar kaydı atla
            // Take → belirtilen kadar kayıt al
            // Bu ikisi birlikte pagination oluşturur

            return await query.ToListAsync();
            // Artık query çalıştırılır ve DB'den veri çekilir

        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);
            // Global filter otomatik !IsDeleted ekler
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            // AsNoTracking:
            // EF normalde çektiği verileri takip eder (tracking).
            // Ama burada sadece veri okuyacağız (update yok).
            // Bu yüzden performans için tracking kapatıyoruz.

            // ToListAsync:
            // Veritabanına gidip tüm kategorileri çeker.
            // Bu işlem IO (database) olduğu için zaman alabilir.
            // Bu yüzden async kullanıyoruz.

            var values = await _context.Categories
                .AsNoTracking() // performans için
                .ToListAsync(); // DB'ye gider → await gerekir



            // Entity listesi (Category) olarak geri döndürüyoruz.
            return values;
        }

        public async Task<List<Category>> GetAllCategoriesForTreeAsync()
        {
            // Hiyerarşik yapıda tüm kategorileri getireceğiz.
            return await _context.Categories
        .Where(c => !c.IsDeleted)
        .Include(c => c.ParentCategory) // ParentCategoryName için gerekli
        .Include(c => c.Products)       // ProductCount için gerekli
        .AsNoTracking()
        .ToListAsync();
        }

        //id'ye göre kategori getirirken alt kategorilerini de dahil et
        public Task<Category?> GetCategoryWithSubAsync(int id)
        {
            return _context.Categories
                .Include(c => c.SubCategories)//Include nedir?                 // Include, ilişkili verileri de çekmek için kullanılır. Peki neden kullanırız? Normalde bir Category çektiğimizde, onun SubCategories'ı gelmez. Eğer alt kategorileri de görmek istiyorsak Include ile onları da dahil ederiz. //ProductName Incude etmeden sadece CategoryName gelirdi, şimdi SubCategories da geliyor.
                .FirstOrDefaultAsync(c => c.CategoryId == id && !c.IsDeleted);
        }

        public async Task<bool> IsCategoryNameExistsForUpdateAsync(int categoryId, string categoryName)
        {
            categoryName = categoryName.Trim();
            // Kullanıcının başta ve sonda boşluk bırakma ihtimaline karşı temizliyoruz
            //
            // Örnek:
            // " Elektronik " -> "Elektronik"

            return await _context.Categories.AnyAsync(x =>
                x.CategoryId != categoryId &&
                x.CategoryName.ToLower() == categoryName.ToLower());
            // Categories tablosunda şu şartı sağlayan kayıt var mı diye bakıyoruz:
            //
            // 1) x.CategoryId != categoryId
            //    -> Güncellenen kaydın kendisini dışarıda bırak
            //
            // 2) x.CategoryName.ToLower() == categoryName.ToLower()
            //    -> Aynı isimde başka kayıt var mı kontrol et
            //
            // AnyAsync:
            // En az bir tane eşleşen kayıt varsa true döner
            // Hiç yoksa false döner
        }

        public async Task<bool> IsCatergoyNameExistsAsync(string categoryName, int id = 0)
        {
            categoryName = categoryName.Trim();
            //Kullanıcının başına veya sonuna boşluk koyma ihtimaline karşın Trim ile temizliyoruz.

            return await _context.Categories.AnyAsync(x =>
            x.CategoryName.ToLower() == categoryName.ToLower() &&
            x.CategoryId != id &&
            !x.IsDeleted
            );
            // Categories tablosunda, verilen categoryName ile aynı ada sahip kayıt var mı diye bakıyoruz
            //
            // AnyAsync ne yapar?
            // En az bir tane eşleşen kayıt varsa true döner
            // Hiç yoksa false döner
        }

        public async Task SaveChangesAsyncR()
        {
            await _context.SaveChangesAsync();
        }

        private async Task DeleteRecursiveInternalAsync(Category entity)
        {
            await _context.Entry(entity)
                .Collection(c => c.SubCategories)
                .LoadAsync();

            entity.IsDeleted = true;

            foreach (var subCategory in entity.SubCategories)
            {
                await DeleteRecursiveInternalAsync(subCategory);
            }
        }

        public async Task UpdateAsync(Category entity)
        {

            //Update sadece işaretleme yapar
            //Veritabanına gitmez, RAM'de çalışır-->Bu yüzden await değil.
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task TransferSubCategoriesAsync(Category category, int? newParentId)
        {
            await _context.Entry(category)
                .Collection(c => c.SubCategories)
                .LoadAsync();

            foreach (var subCategory in category.SubCategories)
            {
                subCategory.ParentCategoryId = newParentId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.
                AnyAsync(c => c.CategoryId == id);
        }
    }
}
