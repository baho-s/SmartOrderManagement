using FluentValidation;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Features.Categories.Command.CreateCategory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators
{
    public interface ICreateCategoryValidator:IValidator<CreateCategoryCommand>
    {

    }
}
