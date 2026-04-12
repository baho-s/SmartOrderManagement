using MediatR;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Query.GetCustomerList
{
    public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, List<CustomerListDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerListQueryHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CustomerListDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0)
            {
                request.PageNumber = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = 10;
            }
            var customerListDtos = new List<CustomerListDto>();

            var customers = await _customerRepository.GetAllAsync(request.PageNumber, request.PageSize);
            
            foreach(var customer in customers)
            {
                customerListDtos.Add(new CustomerListDto
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address
                });
            }
            return customerListDtos;
        }
    }
}
