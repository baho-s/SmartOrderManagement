using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductIsActive
{
    public class UpdateProductIsActiveCommand : IRequest,ICacheInvalidator
    {
        public int ProductId { get; set; }
        public bool NewIsActive { get; init; }

        public List<string> CacheKeysToRemove => new()
        {
                $"product-{ProductId}"
        };
    }
}
