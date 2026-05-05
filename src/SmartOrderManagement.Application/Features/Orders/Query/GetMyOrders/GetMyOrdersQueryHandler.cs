using MediatR;
using Microsoft.AspNetCore.Http;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Orders.Query.GetMyOrders
{
    public class GetMyOrdersQueryHandler:IRequestHandler<GetMyOrdersQuery, List<GetMyOrdersDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyOrdersQueryHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetMyOrdersDto>> Handle(GetMyOrdersQuery query,CancellationToken cancellationToken)
        {
            // 1. Token'dan CustomerId oku
            var customerIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("CustomerId");

            if (customerIdClaim == null)
                throw new Exception("Kullanıcı girişi yapılmamış.");

            var customerId = int.Parse(customerIdClaim.Value);
            // Token'dan gelen CustomerId'yi int'e çevirdik

            // 2. O müşteriye ait siparişleri getir
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            // Henüz bu metodu yazmadık, sonraki adımda ekleyeceğiz

            // 3. DTO'ya çevir
            return orders.Select(o => new GetMyOrdersDto
            {
                OrderId = o.OrderId,
                Address = o.Address,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                PaymentStatus = o.PaymentStatus,
                OrderItems = o.OrderItems.Select(oi => new GetMyOrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            }).ToList();
        }
    }
}
