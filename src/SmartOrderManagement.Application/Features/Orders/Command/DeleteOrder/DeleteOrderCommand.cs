using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.DeleteOrder
{
    public record DeleteOrderCommand:IRequest
    {
        public int OrderId { get; init; }
    }
}
