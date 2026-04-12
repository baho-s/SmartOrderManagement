using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomerAddress
{
    public class UpdateCustomerAddressCommand:IRequest
    {
        public int CustomerId { get; set; }
        public string NewAddress { get; init; } = string.Empty;

    }
}
