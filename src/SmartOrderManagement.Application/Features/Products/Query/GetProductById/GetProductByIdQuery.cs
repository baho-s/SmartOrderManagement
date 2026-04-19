using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductByIdDto>,ICacheableQuery
    {
        public int ProductId { get; set; }

        [BindNever]
        public string CacheKey => $"product-{ProductId}";

        [BindNever]
        public TimeSpan AbsoluteExpiration => TimeSpan.FromSeconds(240);
    }
}
