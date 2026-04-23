using MediatR;
using MediatR.NotificationPublishers;
using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.CreateOrder
{
    public record CreateOrderCommand:IRequest<int>,ICacheInvalidator
    {
        //CustomerId'yi dışarıdan almayacağız,token'dan alacağız. O yüzden bu property'yi kaldırıyoruz.
        public string Address { get; init; } = string.Empty;
        public List<CreateOrderItemDto> CreateOrderItems { get; init; }
        //Veri alırken CreateOrderItemDto üzerinden alıp,mapleme yaparak OrderItem nesnesine dönüştüreceğiz.
        //Böylece sadece ihtiyacımız olan verileri alırız. Otomatik doldurulan property'leri dışardan almayız.

        public List<string> CacheKeysToRemove => new()
        {
             $"product-{string.Join(",", CreateOrderItems.Select(oi => oi.ProductId))}",
             $"products"
        };        
        
    }
}
