using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomerEmail
{
    public class UpdateCustomerEmailPhoneCommand:IRequest
    {
        public int CustomerId { get; set; }
        public string NewEmail { get; init; }=string.Empty;
        public string NewPhone { get; init; }=string.Empty;
    }
}
