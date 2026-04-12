using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductCategoryId
{
    public class UpdateProductCategoryIdCommand:IRequest
    {
        public int ProductId { get; set; }
        public int NewCategoryId { get; init; }
    }
}
