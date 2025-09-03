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
        }
    }
}
