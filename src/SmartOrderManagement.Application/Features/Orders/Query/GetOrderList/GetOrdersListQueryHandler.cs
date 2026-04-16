using MediatR;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Query.GetOrderList
{
    public class GetOrdersListQueryHandler:IRequestHandler<GetOrdersListQuery,List<OrderListDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderListDto>> Handle(GetOrdersListQuery request,CancellationToken cancellationToken)
        {
            //ordersQuery=_context.Orders.AsQueryable();
            //ordersQuery üzerinde filtreleme, sıralama ve sayfalama
            //işlemleri yaparak OrderListDto nesnelerine dönüştürün

            if(request.PageNumber<=0)
            {
                request.PageNumber=1;
            }
            if(request.PageSize<=0)
            {
                request.PageSize=10;
            }

                var orders = await _orderRepository.GetOrdersListAsync(request.PageNumber, request.PageSize);
            List<OrderListDto> ordersList = new();

            //Algoritma mantığı geliştirmek için maplemeyi el ile yapmaya devam ediyorum.
            foreach (var order in orders)
            {
                ordersList.Add(new OrderListDto
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    PaymentStatus = order.PaymentStatus,
                    Address=order.Address,
                    OrderItems =order.OrderItemsList(order).Select(oi=>new OrderItemListDto
                    {
                        OrderItemId=oi.OrderItemId,
                        ProductId=oi.ProductId,
                        Quantity=oi.Quantity,
                        UnitPrice=oi.UnitPrice
                    }).ToList()

                });
            }

            return ordersList;
        }
    }
}
