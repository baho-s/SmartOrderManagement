using FluentValidation;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators
{
    public interface ICreateCategoryValidator:IValidator<CreateCategoryDto>
    {

    }
}
