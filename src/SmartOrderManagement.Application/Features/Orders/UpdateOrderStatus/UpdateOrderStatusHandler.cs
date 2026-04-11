using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Domain.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orderds.UpdateOrderStatus
{
    public class UpdateOrderStatusHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
        }

        public async Task Handle(UpdateOrderStatusCommand command)
        {
            if(command.OrderId<=0)
            {
                throw new Exception("Geçersiz sipariş ID'si");
            }
            var order=await _orderRepository.GetByIdAsync(command.OrderId);
            if(order==null)
            {
                throw new Exception("Order not found");
            }
            switch (command.NewStatus)
            {
                case OrderStatus.Tamamlandi:
                    order.OrderStatusTamamlandi();
                    break;
                case OrderStatus.Iptal:

                    order.OrderStatusIptal();
                    break;
                default:
                    throw new Exception("Geçersiz sipariş durumu");
            }
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }
    }
}
