using AutoMapper;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Mappings
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            //CreateMap<src(kaynak nesne), dest(hedef nesne)>();
            //kaynak nesne nedir? Hangi nesneden veri alacağız?
            //hedef nesne nedir? Hangi nesneye veri aktaracağız?
            CreateMap<CreateOrderDto, Order>();

            CreateMap<Order, OrderByIdDto>();

            CreateMap<Order, OrderListDto>();

            CreateMap<UpdateOrderDto, Order>();
        }
    }
}
