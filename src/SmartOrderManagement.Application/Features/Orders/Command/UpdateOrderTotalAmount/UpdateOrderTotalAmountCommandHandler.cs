using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderTotalAmount
{
    public class UpdateOrderTotalAmountCommandHandler:IRequestHandler<UpdateOrderTotalAmountCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderTotalAmountCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateOrderTotalAmountCommand command, CancellationToken cancellationToken)
        {
            if (command.OrderId < 0)
            {
                throw new Exception("OrderId negatif olamaz.");
            }

            var order = await _orderRepository.GetByIdAsync(command.OrderId);

            if (order == null)
            {
                throw new Exception($"Order id bulunamadı: {command.OrderId}");
            }
            order.UpdateOrderTotalAmount(command.NewTotalAmount);
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }
    }
}
