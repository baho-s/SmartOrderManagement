using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.DeleteOrder
{
    public class DeleteOrderCommandHandler:IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;//readonly bir alanın birkez atanabileceğini ve birdaha değiştirilmeyeceğini belirtir.

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteOrderCommand command,CancellationToken cancellationToken)
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

            if(order.Status==Domain.Enums.OrderEnums.OrderStatus.Iptal)
            {
                await _orderRepository.DeleteAsync(order);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("Sadece iptal edilmiş siparişler silinebilir.");
            }            
        }
    }
}
