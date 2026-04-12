using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductIsActive
{
    public class UpdateProductIsActiveCommand : IRequest
    {
        public int ProductId { get; set; }
        public bool NewIsActive { get; init; } 
    }
}
