using MediatR;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Application.Interfaces.Validators.CustomerValidators;
using SmartOrderManagement.Application.Interfaces.Validators.ProductValidators;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Command.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICreateCustomerValidator _createCustomerValidator;  

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, ICreateCustomerValidator createCustomerValidator)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _createCustomerValidator = createCustomerValidator;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _createCustomerValidator.ValidateAsync(request);
            if(!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationMyException(validationErrors);
            }

            var customer=new Customer(request.FullName,request.Phone,request.Email,request.Address);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.CommitAsync();
            return customer.CustomerId;
        }
    }
}
