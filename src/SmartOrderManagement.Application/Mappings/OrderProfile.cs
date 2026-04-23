using AutoMapper;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Application.Features.Orders.Command.CreateOrder;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            //CreateMap<src(kaynak nesne), dest(hedef nesne)>();
            //kaynak nesne nedir? Hangi nesneden veri alacağız?
            //hedef nesne nedir? Hangi nesneye veri aktaracağız?

            /*CreateMap<CreateOrderCommand, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                .ConstructUsing(src => new Order (src.Address));*/
            // Şunu yapar:
            // 1. new Order(command.CustomerId) ile oluştur
            // 2. Aynı isimli property'leri otomatik eşleştir
            // 3. OrderItems'a dokunma, biz Handler'da hallederiz

            // Mapper olmadan elle yapsan şöyle yapardın:
            //var order = new Order(command.CustomerId);
            //order.CustomerName = command.CustomerName;
            //order.OrderDate = command.OrderDate;
            // ... her property'yi tek tek
            // Mapper bunu otomatik yapar!
            //var order = _mapper.Map<Order>(command);

            CreateMap<Order, OrderByIdDto>();

            CreateMap<Order, OrderListDto>();

            CreateMap<UpdateOrderDto, Order>();
        }
    }
}
