using FluentValidation;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Validators.CategoryValidators
{
    public class UpdateCategoryValidator:AbstractValidator<UpdateCategoryDto>,IUpdateCategoryValidator
    {
        public UpdateCategoryValidator()
        {
            //Id 0 veya negatif olamaz
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori id değeri giriniz.");

            RuleFor(x => x.CategoryName)
                .Cascade(CascadeMode.Stop)// CascadeMode.Stop = İlk hatada dur, diğer kuralları çalıştırma
                .Must(name => !string.IsNullOrWhiteSpace(name))//Must: FluentValidation'da özel kural yazmamızı sağlar,İçine yazılan ifade true ise başarılı false ise validation hatalı kabul edilir.
                .WithMessage("Kategori adı boş veya sadece boşluklardan oluşamaz.")
                .MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kategori adı en fazla 50 karakter olmalıdır.");
        }
    }
}
