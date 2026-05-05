using FluentValidation;
using SmartOrderManagement.Application.Features.Orders.Command.CreateOrder;
using SmartOrderManagement.Application.Interfaces.Validators.OrderValidators;

namespace SmartOrderManagement.Application.Validators.OrderValidators
{
    public class CreateOrderValidator:AbstractValidator<CreateOrderCommand>,ICreateOrderValidator
    {
        public CreateOrderValidator()
        {
             

               RuleFor(x => x.CreateOrderItems)
                .NotEmpty().WithMessage("Sipariş öğeleri boş olamaz.")
                .ForEach(item =>
                {
                    item.ChildRules(orderItem =>
                    {
                        orderItem.RuleFor(x => x.Quantity)
                            .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır.");
                        orderItem.RuleFor(x => x.ProductId)
                            .GreaterThan(0).WithMessage("ProductId 0'dan büyük olmalıdır.");
                    });
                });

        }
    }
}
