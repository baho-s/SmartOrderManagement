using MediatR;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductByIdDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductByIdDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.ProductId <= 0)
            {
                throw new ArgumentException("Id 0 veya negatif olamaz.");
            }
            var product= await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new Exception($"Id'ye ait ürün bulunumadı: {request.ProductId}");
            }
            var productDto = new ProductByIdDto{
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductStock = product.ProductStock,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                CategoryName = product.Category.CategoryName
            };
            

            return productDto;
        }
    }
}
