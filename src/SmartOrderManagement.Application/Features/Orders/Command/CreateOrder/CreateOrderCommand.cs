using MediatR;
using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.CreateOrder
{
    public record CreateOrderCommand:IRequest<int>
    {
        //CustomerId'yi dışarıdan almayacağız,token'dan alacağız. O yüzden bu property'yi kaldırıyoruz.
        public string Address { get; init; } = string.Empty;
        public List<CreateOrderItemDto> CreateOrderItems { get; init; }
        //Veri alırken CreateOrderItemDto üzerinden alıp,mapleme yaparak OrderItem nesnesine dönüştüreceğiz.
        //Böylece sadece ihtiyacımız olan verileri alırız. Otomatik doldurulan property'leri dışardan almayız.
    }
}
