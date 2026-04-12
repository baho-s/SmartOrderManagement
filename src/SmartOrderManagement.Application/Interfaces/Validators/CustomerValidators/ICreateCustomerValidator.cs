using FluentValidation;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Application.Features.Customers.Command.CreateCustomer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Validators.CustomerValidators
{
    public interface ICreateCustomerValidator:IValidator<CreateCustomerCommand>
    {
    }
}
