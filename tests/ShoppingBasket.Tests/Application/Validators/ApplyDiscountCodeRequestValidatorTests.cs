using FluentValidation.TestHelper;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Validators;

namespace ShoppingBasket.Tests.Application.Validators
{
    public class ApplyDiscountCodeRequestValidatorTests
    {
        private readonly ApplyDiscountCodeRequestValidator _validator;

        public ApplyDiscountCodeRequestValidatorTests()
        {
            _validator = new ApplyDiscountCodeRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenCodeIsValid()
        {
            // Arrange
            var request = new ApplyDiscountCodeRequest("SUMMER20");

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Validate_ShouldFail_WhenCodeIsEmptyOrNull(string invalidCode)
        {
            // Arrange
            var request = new ApplyDiscountCodeRequest(invalidCode);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                  .WithErrorMessage("Discount code is required");
        }
    }
}
