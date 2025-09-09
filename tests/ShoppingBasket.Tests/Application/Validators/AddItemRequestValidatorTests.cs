using FluentValidation.TestHelper;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Validators;

namespace ShoppingBasket.Tests.Application.Validators
{
    public class AddItemRequestValidatorTests
    {
        private readonly AddItemRequestValidator _validator;

        public AddItemRequestValidatorTests()
        {
            _validator = new AddItemRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenRequestIsValid()
        {
            // Arrange
            var request = new AddItemRequest(Guid.NewGuid(), "Product A", 10m, "GBP", 2, 20);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ShouldFail_WhenProductIdIsEmpty()
        {
            // Arrange
            var request = new AddItemRequest(Guid.Empty, "Product A", 10m, "GBP", 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductId)
                  .WithErrorMessage("ProductId is required.");
        }

        [Fact]
        public void Validate_ShouldFail_WhenProductNameIsEmpty()
        {
            // Arrange
            var request = new AddItemRequest(Guid.NewGuid(), "", 10m, "GBP", 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductName)
                  .WithErrorMessage("ProductName is required.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Validate_ShouldFail_WhenUnitPriceIsNotPositive(decimal invalidPrice)
        {
            // Arrange
            var request = new AddItemRequest(Guid.NewGuid(), "Product A", invalidPrice, "GBP", 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UnitPrice);
        }

        [Fact]
        public void Validate_ShouldFail_WhenQuantityIsZeroOrLess()
        {
            // Arrange
            var request = new AddItemRequest(Guid.NewGuid(), "Product A", 10m, "GBP", 0);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                  .WithErrorMessage("Quantity must be at least 1.");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(150)]
        public void Validate_ShouldFail_WhenDiscountPercentageOutOfRange(decimal invalidDiscount)
        {
            // Arrange
            var request = new AddItemRequest(Guid.NewGuid(), "Product A", 10m, "GBP", 1, invalidDiscount);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DiscountPercentage)
                  .WithErrorMessage("DiscountPercentage must be between 0 and 100 if provided.");
        }

        [Fact]
        public void Validate_ShouldPass_WhenDiscountPercentageIsNull()
        {
            // Arrange
            var request = new AddItemRequest(Guid.NewGuid(), "Product A", 10m, "GBP", 1, null);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.DiscountPercentage);
        }
    }
}
