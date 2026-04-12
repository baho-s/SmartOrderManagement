using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;

namespace SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderAddress
{
    public class UpdateOrderAddressCommandHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderAddressCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateOrderAddressCommand command)
        {
            if(command.OrderId<0)
            {
                throw new ValidationMyException("OrderId negatif olamaz.");
            }

            var order=await _orderRepository.GetByIdAsync(command.OrderId);

            if(order is null)
            {
                throw new NotFoundException($"Order id bulunamadı: {command.OrderId}");
            }

            order.UpdateOrderAddress(command.NewAddress);
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync();

        }
    }
}
