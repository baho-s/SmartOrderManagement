using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductCategoryId
{
    public class UpdateProductCategoryIdCommand:IRequest, ICacheInvalidator
    {
        public int ProductId { get; set; }
        public int NewCategoryId { get; init; }

        public List<string> CacheKeysToRemove => new()
        {
                $"product-{ProductId}"
        };
    }
}
