using FluentValidation;
using ShoppingBasket.Application.Contracts;

namespace ShoppingBasket.Application.Validators
{
    public sealed class AddMultipleItemsRequestValidator : AbstractValidator<AddMultipleItemsRequest>
    {
        public AddMultipleItemsRequestValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item must be provided.");

            // Apply AddItemRequestValidator to each item
            RuleForEach(x => x.Items).SetValidator(new AddItemRequestValidator());
        }
    }
}
