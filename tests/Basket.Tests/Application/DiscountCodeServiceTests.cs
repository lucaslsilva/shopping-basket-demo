using FluentAssertions;
using ShoppingBasket.Application.Services;

namespace ShoppingBasket.Tests.Application
{
    public class DiscountCodeServiceTests
    {
        private readonly DiscountCodeService _service = new();

        [Theory]
        [InlineData("SUMMER20", 20)]
        [InlineData("WELCOME10", 10)]
        public void Validate_ValidCode_ShouldReturnDiscountCode(string code, decimal expectedPercentage)
        {
            // Act
            var result = _service.Validate(code);

            // Assert
            result.Code.Should().Be(code);
            result.Percentage.Should().Be(expectedPercentage);
        }

        [Fact]
        public void Validate_InvalidCode_ShouldThrow()
        {
            // Act
            Action act = () => _service.Validate("INVALID");

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Invalid discount code: INVALID");
        }
    }
}
