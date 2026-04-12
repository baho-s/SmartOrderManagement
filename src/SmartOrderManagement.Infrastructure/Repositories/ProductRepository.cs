using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task DeleteAsync(Product product)
        {
            product.IsDeleted = true;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var value=await _context.Products.Include(x=>x.Category).FirstOrDefaultAsync(x => x.ProductId == id);
            return value;
        }

        public async Task<List<Product>> GetProductsAsync(byte pageNumber, byte pageSize)
        {
            return await _context.Products
                .Include(x=>x.Category)//CategoryName için gerekli.
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
