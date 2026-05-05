using MediatR;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Query.GetOrderList
{
    public class GetOrdersListQuery:IRequest<List<OrderListDto>>
    {
        public byte PageNumber { get; set; } = 1;//Sayfa numarası
        public byte PageSize { get; set; } = 10;//Sayfa başına düşen kayıt sayısı
    }
}
