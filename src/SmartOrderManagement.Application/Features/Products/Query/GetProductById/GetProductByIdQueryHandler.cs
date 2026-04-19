using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Features.Products.Query.GetProductList;
using SmartOrderManagement.Application.Interfaces.Caching;
using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductByIdDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private readonly ICacheKeyTracker _cacheKeyTracker;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMemoryCache cache, ICacheKeyTracker cacheKeyTracker)
        {
            _productRepository = productRepository;
            _cache = cache;
            _cacheKeyTracker = cacheKeyTracker;
        }

        public async Task<ProductByIdDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.ProductId <= 0)
            {
                throw new ValidationMyException("Id 0 veya negatif olamaz.");
            }
            
            var allKeys= _cacheKeyTracker.GetKeys();

            foreach(var key in allKeys)
            {
                if(_cache.TryGetValue(key,out List<ProductListDto> deneme) && deneme.Count > 0)
                {
                    foreach(var item in deneme)
                    {
                        if(item.ProductId == request.ProductId)
                        {
                            var dto = new ProductByIdDto{
                                ProductId = item.ProductId,
                                ProductName = item.ProductName,
                                ProductPrice = item.ProductPrice,
                                ProductStock = item.ProductStock,
                                CategoryId = item.CategoryId,
                                IsActive = item.IsActive,
                                CategoryName = item.CategoryName
                            };
                            return dto;
                        }
                    }
                }

            }

            var product= await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Id'ye ait ürün bulunumadı: {request.ProductId}");
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
