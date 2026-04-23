using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProduct
{
    public class UpdateProductCommand:IRequest,ICacheInvalidator
    {
        public int ProductId { get; set; }
        public string NewProductName { get; init; }
        public decimal NewProductPrice { get; init; }
        public int NewProductStock { get; init; }
        public bool NewIsActive { get; init; }
        public int NewCategoryId { get; init; }

        public List<string> CacheKeysToRemove => new()
        {
            $"product-{ProductId}"
        };
    }
}
