using MediatR;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomerEmail
{
    public class UpdateCustomerEmailPhoneCommandHandler : IRequestHandler<UpdateCustomerEmailPhoneCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerEmailPhoneCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCustomerEmailPhoneCommand request, CancellationToken cancellationToken)
        {
            var customer= await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                throw new NotFoundException($"Id'ye ait müşteri bulunamadı: {request.CustomerId}");
            }
            customer.UpdateCustomerEmailAndPhone(request.NewEmail, request.NewPhone);
            _customerRepository.Update(customer);
            await _unitOfWork.CommitAsync();
        }
    }
}
