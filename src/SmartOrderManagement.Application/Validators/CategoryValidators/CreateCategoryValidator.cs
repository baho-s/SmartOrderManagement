using FluentValidation;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Features.Categories.Command.CreateCategory;
using SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Validators.CategoryValidators
{
    public class CreateCategoryValidator:AbstractValidator<CreateCategoryCommand>,ICreateCategoryValidator
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Kategori adı boş geçilemez.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Kategori adı sadece boşluklardan oluşamaz.")
            .MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Kategori adı en fazla 50 karakter olmalıdır.");
        }
    }
}
