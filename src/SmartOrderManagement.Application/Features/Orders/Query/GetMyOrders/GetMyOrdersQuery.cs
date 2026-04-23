using MediatR;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Orders.Query.GetMyOrders
{
    public class GetMyOrdersQuery:IRequest<List<GetMyOrdersDto>>
    {
        // Boş çünkü CustomerId token'dan okunacak
        // Kullanıcı hiçbir şey göndermek zorunda değil
    }
}
