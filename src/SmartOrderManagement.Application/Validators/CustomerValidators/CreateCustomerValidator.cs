using FluentValidation;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Application.Interfaces.Validators.CustomerValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Validators.CustomerValidators
{
    public class CreateCustomerValidator:AbstractValidator<CreateCustomerDto>,ICreateCustomerValidator
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soayad boş geçilemez.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Ad Soyad sadece boşluklardan oluşamaz.")
            .MinimumLength(2).WithMessage("Ad Soyad en az 2 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Ad Soyad en fazla 50 karakter olmalıdır.");

        }
    }
}
