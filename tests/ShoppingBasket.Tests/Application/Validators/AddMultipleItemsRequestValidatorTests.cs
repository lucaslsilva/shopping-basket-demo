using FluentValidation.TestHelper;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Validators;

namespace ShoppingBasket.Tests.Application.Validators
{
    public class AddMultipleItemsRequestValidatorTests
    {
        private readonly AddMultipleItemsRequestValidator _validator;

        public AddMultipleItemsRequestValidatorTests()
        {
            _validator = new AddMultipleItemsRequestValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenItemsCollectionIsEmpty()
        {
            // Arrange
            var request = new AddMultipleItemsRequest(new List<AddItemRequest>());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Items)
                  .WithErrorMessage("At least one item must be provided.");
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllItemsAreValid()
        {
            // Arrange
            var validItem = new AddItemRequest(Guid.NewGuid(), "Product A", 10m, "GBP", 1);
            var request = new AddMultipleItemsRequest(new List<AddItemRequest> { validItem });

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ShouldFail_WhenAnyItemIsInvalid()
        {
            // Arrange
            var invalidItem = new AddItemRequest(Guid.Empty, "Product A", 10m, "GBP", 1);
            var validItem = new AddItemRequest(Guid.NewGuid(), "Product B", 10m, "GBP", 1);
            var request = new AddMultipleItemsRequest(new List<AddItemRequest> { validItem, invalidItem });

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor("Items[1].ProductId")
                  .WithErrorMessage("ProductId is required.");
        }
    }
}
