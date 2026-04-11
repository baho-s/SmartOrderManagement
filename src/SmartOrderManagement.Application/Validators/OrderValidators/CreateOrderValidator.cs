using FluentValidation;
using SmartOrderManagement.Application.Features.Orderds.CreateOrder;
using SmartOrderManagement.Application.Interfaces.Validators.OrderValidators;

namespace SmartOrderManagement.Application.Validators.OrderValidators
{
    public class CreateOrderValidator:AbstractValidator<CreateOrderCommand>,ICreateOrderValidator
    {
        public CreateOrderValidator()
        {
                RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId 0'dan büyük olmalıdır.");

               RuleFor(x => x.CreateOrderItems)
                .NotEmpty().WithMessage("Sipariş öğeleri boş olamaz.")
                .ForEach(item =>
                {
                    item.ChildRules(orderItem =>
                    {
                        orderItem.RuleFor(x => x.Quantity)
                            .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır.");
                        orderItem.RuleFor(x => x.UnitPrice)
                            .GreaterThan(0).WithMessage("Birim fiyat 0'dan büyük olmalıdır.");
                        orderItem.RuleFor(x => x.TotalPrice)
                            .GreaterThan(0).WithMessage("Toplam fiyat 0'dan büyük olmalıdır.");
                        orderItem.RuleFor(x => x.ProductId)
                            .GreaterThan(0).WithMessage("ProductId 0'dan büyük olmalıdır.");
                    });
                });

        }
    }
}
