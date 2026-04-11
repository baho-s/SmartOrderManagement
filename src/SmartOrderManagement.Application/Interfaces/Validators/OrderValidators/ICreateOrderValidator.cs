using FluentValidation;
using SmartOrderManagement.Application.Features.Orderds.CreateOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Validators.OrderValidators
{
    public interface ICreateOrderValidator:IValidator<CreateOrderCommand>
    {
    }
}
