using MediatR;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Query.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerByIdDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerByIdDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.CustomerId <= 0)
            {
                throw new ArgumentException($"Geçersiz müşteri Id'si: {request.CustomerId}");
            }
            var customer=await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException($"Müşteri bulunamadı: {request.CustomerId}");
            }
            var customerByIdDto=new CustomerByIdDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address
            };
            return customerByIdDto;
        }
    }
}
