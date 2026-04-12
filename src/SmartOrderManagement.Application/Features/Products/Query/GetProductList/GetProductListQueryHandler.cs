using AutoMapper.Execution;
using MediatR;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductList
{
    public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, List<ProductListDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductListQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductListDto>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0)
            {
                request.PageNumber = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = 10;
            }

            var productList = new List<ProductListDto>();
            var products = await _productRepository.GetProductsAsync(request.PageNumber, request.PageSize);
            //Algoritma mantığını güçlendirmek için El ile maplemeye devam ediyoruz.
            //AutoMapper'de de nasıl maplendiğine bakıyorum.
            foreach (var product in products)
            {
                var productDto = new ProductListDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductStock = product.ProductStock,
                    IsActive = product.IsActive,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName
                };
                productList.Add(productDto);
            }
            return productList; 
        }
    }
}
