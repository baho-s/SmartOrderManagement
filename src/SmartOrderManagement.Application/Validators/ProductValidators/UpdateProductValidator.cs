using FluentValidation;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Interfaces.Validators.ProductValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Validators.ProductValidators
{
    public class UpdateProductValidator:AbstractValidator<UpdateProductDto>,IUpdateProductValidator
    {
        public UpdateProductValidator()
        {
            RuleFor(x=>x.ProductId)
                .GreaterThan(0).WithMessage("Geçerli bir ProductId giriniz.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir CategoryId giriniz.");

            RuleFor(x => x.ProductName)
                .Cascade(CascadeMode.Stop)// CascadeMode.Stop = İlk hatada dur, diğer kuralları çalıştırma
                .Must(name => !string.IsNullOrWhiteSpace(name))//Must: FluentValidation'da özel kural yazmamızı sağlar,İçine yazılan ifade true ise başarılı false ise validation hatalı kabul edilir.
                .WithMessage("Ürün adı boş veya sadece boşluklardan oluşamaz.")
                .MinimumLength(2).WithMessage("Ürün adı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ürün adı en fazla 50 karakter olmalıdır.");
        }
    }
}
