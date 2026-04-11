using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using SmartOrderManagement.Application.DTOs.OrderItemDtos;

namespace SmartOrderManagement.Application.Features.Orderds.GetOrderById
{
    public class GetOrderByIdHandler
    {
        private readonly IOrderRepository _orderRepository;
        

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderByIdDto> Handle(GetOrderByIdQuery query)
        {
            var order = await _orderRepository.GetByIdAsync(query.OrderId);
            if (order == null)
            {
                throw new NotFoundException($"Order id bulunamadı: {query.OrderId}");
            }

            var orderItems = order.OrderItems.Select(oi => new OrderItemListDto
            {
                OrderItemId = oi.OrderItemId,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList();
            
            var orderByIdDto = new OrderByIdDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                CustomerId = order.CustomerId,
                OrderItems = orderItems
            };
            return orderByIdDto;
        }
    }
}
