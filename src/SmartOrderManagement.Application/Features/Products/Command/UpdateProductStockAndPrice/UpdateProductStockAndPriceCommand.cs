using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductStockAndPrice
{
    public class UpdateProductStockAndPriceCommand:IRequest,ICacheInvalidator
    {
        public int ProductId { get; set; }
        public decimal NewPrice { get; init; }
        public int NewStock { get; init; }

        public List<string> CacheKeysToRemove => new()
        {
                $"product-{ProductId}"
        };
    }
}
