using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomer
{
    public class UpdateCustomerCommand:IRequest
    {
        public int CustomerId { get; set; }

        public string NewFullName { get; init; } = string.Empty;
        public string NewPhone { get; init; } = string.Empty;
        public string NewEmail { get; init; } = string.Empty;
        public string NewAddress { get; init; } = string.Empty;
    }
}
