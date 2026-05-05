using MediatR;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Query.GetOrderById
{
    public record GetOrderByIdQuery:IRequest<OrderByIdDto>
    {
        public int OrderId { get; init; }
    }
}
