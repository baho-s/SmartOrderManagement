using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orderds.CreateOrder
{
    public class CreateOrderCommand
    {
        public int CustomerId { get; set; }
        public string Address { get; set; } = string.Empty;
        public List<CreateOrderItemDto> CreateOrderItems { get; set; }
        //Veri alırken CreateOrderItemDto üzerinden alıp,mapleme yaparak OrderItem nesnesine dönüştüreceğiz.
        //Böylece sadece ihtiyacımız olan verileri alırız. Otomatik doldurulan property'leri dışardan almayız.
    }
}
