using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.DeleteProduct
{
    public record DeleteProductCommand
    {
        public int ProductId { get; init; }
    }
}
