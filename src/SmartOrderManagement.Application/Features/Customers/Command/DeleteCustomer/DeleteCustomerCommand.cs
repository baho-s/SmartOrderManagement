using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Command.DeleteCustomer
{
    public record DeleteCustomerCommand:IRequest
    {
        public int CustomerId { get; init; }
    }
}
