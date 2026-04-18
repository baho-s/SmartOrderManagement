using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdatePorductName
{
    public class UpdateProductNameCommand:IRequest, ICacheInvalidator
    {
        public int ProductId { get; set; }
        public string NewProductName { get; init; }

        public List<string> CacheKeysToRemove => new()
        {
            "products"
        };
    }
}
