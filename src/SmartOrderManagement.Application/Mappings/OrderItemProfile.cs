using AutoMapper;
using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Mappings
{
    public class OrderItemProfile:Profile
    {
        public OrderItemProfile()
        {
            //CreateMap<src(kaynak nesne), dest(hedef nesne)>();
            //kaynak nesne nedir? Hangi nesneden veri alacağız?
            //hedef nesne nedir? Hangi nesneye veri aktaracağız?

            CreateMap<CreateOrderItemDto, OrderItem>();
             CreateMap<UpdateOrderItemDto, OrderItem>();
             CreateMap<OrderItem, OrderItemByIdDto>();
             CreateMap<OrderItem, OrderItemListDto>();
        }
    }
}
