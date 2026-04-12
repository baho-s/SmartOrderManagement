using FluentValidation;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Features.Products.Command.CreateProduct;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Validators.ProductValidators
{
    public interface ICreateProductValidator:IValidator<CreateProductCommand>
    {
    }
}
