using FluentValidation;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Validators.CustomerValidators
{
    public interface IUpdateCustomerValidator:IValidator<UpdateCustomerDto>
    {
    }
}
