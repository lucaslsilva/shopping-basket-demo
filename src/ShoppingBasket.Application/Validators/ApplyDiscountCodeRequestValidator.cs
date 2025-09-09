using FluentValidation;
using ShoppingBasket.Application.Contracts;

namespace ShoppingBasket.Application.Validators
{
    public class ApplyDiscountCodeRequestValidator : AbstractValidator<ApplyDiscountCodeRequest>
    {
        public ApplyDiscountCodeRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Discount code is required");
        }
    }
}
