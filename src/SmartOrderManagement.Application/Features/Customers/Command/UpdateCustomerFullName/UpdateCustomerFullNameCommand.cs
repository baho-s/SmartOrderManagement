using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomerFullName
{
    public class UpdateCustomerFullNameCommand : IRequest
    {
        public int CustomerId { get; set; }
        public string NewFullName { get; init; } = string.Empty;
    
    }
}
