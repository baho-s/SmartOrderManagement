using MediatR;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Query.GetCustomerList
{
    public class GetCustomerListQuery:IRequest<List<CustomerListDto>>
    {
        public int PageNumber { get; set; }=1;
        public int PageSize { get; set; }=10;
    }
}
