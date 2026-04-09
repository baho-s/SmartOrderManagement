using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<int> CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(int id,UpdateProductDto updateProductDto);
        Task DeleteProductAsync(int id);
        Task<List<ProductListDto>> GetProductsAsync();
        Task<ProductByIdDto> GetByIdAsync(int id);
    }
}
