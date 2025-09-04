using FluentValidation;
using ShoppingBasket.Application.Contracts;

namespace ShoppingBasket.Application.Validators
{
    public sealed class AddItemRequestValidator : AbstractValidator<AddItemRequest>
    {
        public AddItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("ProductName is required.");

            RuleFor(x => x.UnitPrice)
                .NotNull().WithMessage("UnitPrice is required.")
                .GreaterThan(0).WithMessage("UnitPrice must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100)
                .When(x => x.DiscountPercentage.HasValue).WithMessage("DiscountPercentage must be between 0 and 100 if provided.");
        }
    }
}
