using FluentValidation;
using ShoppingBasket.Application.Contracts;

namespace ShoppingBasket.Application.Validators
{
    public class SetShippingRequestValidator : AbstractValidator<SetShippingRequest>
    {
        public SetShippingRequestValidator()
        {
            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("CountryCode is required")
                .Length(2, 3).WithMessage("CountryCode should be 2 or 3 characters");
        }
    }
}
