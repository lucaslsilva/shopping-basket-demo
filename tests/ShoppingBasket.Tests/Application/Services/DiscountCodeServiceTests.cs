using FluentAssertions;
using ShoppingBasket.Application.Services;

namespace ShoppingBasket.Tests.Application.Services
{
    public class DiscountCodeServiceTests
    {
        private readonly DiscountCodeService _service = new();

        [Theory]
        [InlineData("SUMMER20", 20)]
        [InlineData("WELCOME10", 10)]
        public void Validate_ValidCode_ShouldReturnDiscountCode(string code, decimal expectedPercentage)
        {
            // Arrange
            // (nothing to arrange since service has hardcoded codes)

            // Act
            var result = _service.Validate(code);

            // Assert
            result.Code.Should().Be(code);
            result.Percentage.Should().Be(expectedPercentage);
        }

        [Fact]
        public void Validate_InvalidCode_ShouldThrow()
        {
            // Arrange
            var invalidCode = "INVALID";

            // Act
            Action act = () => _service.Validate(invalidCode);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Invalid discount code: INVALID");
        }

        [Theory]
        [InlineData("summer20", 20)]
        [InlineData("welcome10", 10)]
        public void Validate_ShouldBeCaseInsensitive(string code, decimal expectedPercentage)
        {
            // Arrange
            // (dictionary uses OrdinalIgnoreCase, so case shouldn't matter)

            // Act
            var result = _service.Validate(code);

            // Assert
            result.Percentage.Should().Be(expectedPercentage);
        }
    }
}
