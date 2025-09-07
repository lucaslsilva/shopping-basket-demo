using FluentAssertions;
using ShoppingBasket.Application.Services;

namespace ShoppingBasket.Tests.Application.Services
{
    public class ShippingServiceTests
    {
        private readonly ShippingService _shippingService;

        public ShippingServiceTests()
        {
            _shippingService = new ShippingService();
        }

        [Theory]
        [InlineData("UK", 3)]
        [InlineData("US", 5)]
        [InlineData("DE", 4)]
        [InlineData("FR", 10)] // fallback rule
        public void GetShippingCost_ShouldReturnExpectedAmount(string countryCode, decimal expectedAmount)
        {
            // Arrange
            // (nothing to arrange, service has hardcoded logic)

            // Act
            var result = _shippingService.GetShippingCost(countryCode);

            // Assert
            result.Should().NotBeNull();
            result.Amount.Amount.Should().Be(expectedAmount);
            result.Amount.Currency.Should().Be("GBP");
            result.CountryCode.Should().Be(countryCode.ToUpper());
        }

        [Fact]
        public void GetShippingCost_ShouldBeCaseInsensitive()
        {
            // Arrange
            var input = "uk";

            // Act
            var result = _shippingService.GetShippingCost(input);

            // Assert
            result.Amount.Amount.Should().Be(3m);
            result.Amount.Currency.Should().Be("GBP");
            result.CountryCode.Should().Be("UK");
        }
    }
}
