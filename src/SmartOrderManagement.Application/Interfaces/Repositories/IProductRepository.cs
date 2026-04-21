using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        void Delete(Product product);
        Task<List<Product>> GetProductsAsync(byte pageNumber, byte pageSize);
        Task<Product?> GetByIdAsync(int id);

        Task<List<Product>?> GetProductListByIdCategory(int categoryId);

        Task<Product?> DeleteProductNew(int id);
    }
}
