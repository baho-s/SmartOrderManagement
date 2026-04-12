using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderAddress
{
    public class UpdateOrderAddressCommand
    {
        public int OrderId { get; set; }
        public string NewAddress { get; init; } = string.Empty;
    }
}
