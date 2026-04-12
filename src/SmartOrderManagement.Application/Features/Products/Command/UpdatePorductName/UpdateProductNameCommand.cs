using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdatePorductName
{
    public class UpdateProductNameCommand:IRequest
    {
        public int ProductId { get; set; }
        public string NewProductName { get; init; }
    }
}
