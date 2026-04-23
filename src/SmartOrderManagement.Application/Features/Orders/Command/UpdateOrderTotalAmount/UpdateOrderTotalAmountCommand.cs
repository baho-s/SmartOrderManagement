using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderTotalAmount
{
    //record olursa sadece init ile set edilebilir, class olursa hem init hem de set edilebilir.
    public class UpdateOrderTotalAmountCommand:IRequest
    {
            public int OrderId { get; set; }
            public decimal NewTotalAmount { get; init; }
    }
}
