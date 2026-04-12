using MediatR;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Query.GetCustomerById
{
    public class GetCustomerByIdQuery:IRequest<CustomerByIdDto>
    {
        public int CustomerId { get; init; }
    }
}
