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
        }

        //Alt Kategorisi yoksa bunu kullanarak silme işlemi yapabiliriz.
        public void DeleteAsync(Category entity)
        {
            entity.IsDeleted = true;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {          
            var values = await _context.Categories.Where(c => !c.IsDeleted) 
                .AsNoTracking() 
                .ToListAsync(); 
            return values;
        }

        public async Task<List<Category>> GetAllCategoriesForTreeAsync()
        {
            // Hiyerarşik yapıda tüm kategorileri getireceğiz.
            return await _context.Categories
        .Where(c => !c.IsDeleted)
        .Include(c => c.ParentCategory)
        .Include(c => c.Products)      
        .AsNoTracking()
        .ToListAsync();
        }
   
        public Task<Category?> GetCategoryWithSubAsync(int id)
        {
            return _context.Categories
                .Include(c => c.SubCategories)                                              
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }



        public async Task<bool> IsCategoryNameExistsForUpdateAsync(int categoryId, string categoryName)
        {//Update için aynı isimde başka bir kategori var mı kontrolü yapacağız.
            categoryName = categoryName.Trim();

            return await _context.Categories.AnyAsync(x =>
                x.CategoryId != categoryId &&
                x.CategoryName.ToLower() == categoryName.ToLower());          
        }

        public async Task<bool> IsCatergoyNameExistsAsync(string categoryName, int id = 0)
        {//Yeni kategori eklerken aynı isimde bir kategori var mı kontrolü yapacağız.
            categoryName = categoryName.Trim();

            return await _context.Categories.AnyAsync(x =>
            x.CategoryName.ToLower() == categoryName.ToLower() &&
            x.CategoryId != id &&
            !x.IsDeleted
            );
        }        

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.
                AnyAsync(c => c.CategoryId == id);
        }        
    }
}
