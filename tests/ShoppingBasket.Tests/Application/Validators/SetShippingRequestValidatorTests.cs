using FluentValidation.TestHelper;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Validators;

namespace ShoppingBasket.Tests.Application.Validators
{
    public class SetShippingRequestValidatorTests
    {
        private readonly SetShippingRequestValidator _validator;

        public SetShippingRequestValidatorTests()
        {
            _validator = new SetShippingRequestValidator();
        }

        [Theory]
        [InlineData("US")]
        [InlineData("UK")]
        [InlineData("FRA")]
        public void Validate_ShouldPass_WhenCountryCodeIsValid(string validCode)
        {
            // Arrange
            var request = new SetShippingRequest(validCode);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_ShouldFail_WhenCountryCodeIsEmptyOrNull(string invalidCode)
        {
            // Arrange
            var request = new SetShippingRequest(invalidCode);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CountryCode)
                  .WithErrorMessage("CountryCode is required");
        }

        [Theory]
        [InlineData("U")]    // too short
        [InlineData("FRAN")] // too long
        public void Validate_ShouldFail_WhenCountryCodeLengthIsInvalid(string invalidCode)
        {
            // Arrange
            var request = new SetShippingRequest(invalidCode);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CountryCode)
                  .WithErrorMessage("CountryCode should be 2 or 3 characters");
        }
    }
}
