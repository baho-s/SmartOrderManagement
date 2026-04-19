using MediatR;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductList
{
    public class GetProductListQuery:IRequest<List<ProductListDto>>,ICacheableQuery
    {
        public byte PageNumber { get; set; } = 1;
        public byte PageSize { get; set; } = 10;

        [BindNever]        
        public string CacheKey => $"products-{PageNumber}-{PageSize}";

        [BindNever]        
        public TimeSpan AbsoluteExpiration => TimeSpan.FromSeconds(240);
    }
}
