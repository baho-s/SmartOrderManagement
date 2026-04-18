using MediatR;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductList
{
    public class GetProductListQuery:IRequest<List<ProductListDto>>,ICacheableQuery
    {
        public byte PageNumber { get; set; } = 1;
        public byte PageSize { get; set; } = 10;

        public string CacheKey => $"products-{PageNumber}-{PageSize}";

        public TimeSpan AbsoluteExpiration => TimeSpan.FromSeconds(10);
    }
}
