using FluentValidation;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Validators.ProductValidators
{
    public interface ICreateProductValidator:IValidator<CreateProductDto>
    {
    }
}
