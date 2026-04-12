using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductStockAndPrice
{
    public class UpdateProductStockAndPriceCommand:IRequest
    {
        public int ProductId { get; set; }
        public decimal NewPrice { get; init; }
        public int NewStock { get; init; }
    }
}
