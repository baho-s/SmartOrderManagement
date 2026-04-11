using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.UpdateOrderAddress
{
    public class UpdateOrderAddressCommand
    {
        public int OrderId { get; set; }
        public string NewAddress { get; set; } = string.Empty;
    }
}
