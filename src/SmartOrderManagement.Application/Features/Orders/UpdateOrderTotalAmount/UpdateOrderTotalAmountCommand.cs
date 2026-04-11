using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.UpdateOrderTotalAmount
{
    public class UpdateOrderTotalAmountCommand
    {
            public int OrderId { get; set; }
            public decimal NewTotalAmount { get; set; }
    }
}
