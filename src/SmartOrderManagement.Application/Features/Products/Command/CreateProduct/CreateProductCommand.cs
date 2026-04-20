using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.CreateProduct
{
    public record CreateProductCommand : IRequest<int>, ICacheInvalidator
    {
        //Command'lar DTO'lara benzerler, ancak genellikle sadece gerekli bilgileri içerirler ve genellikle immutable (değiştirilemez) olarak tasarlanırlar.
        public string ProductName { get; init; } = string.Empty;
        public decimal ProductPrice { get; init; }
        public int ProductStock { get; init; }
        public bool IsActive { get; init; }
        public int CategoryId { get; init; }

        public List<string> CacheKeysToRemove => new()
            {
            $"product"
            };
        // Bu ürün hangi kategoriye ait (Foreign Key)
    }
}
