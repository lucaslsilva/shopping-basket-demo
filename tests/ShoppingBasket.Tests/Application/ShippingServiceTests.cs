using FluentAssertions;
using ShoppingBasket.Application.Services;

namespace ShoppingBasket.Tests.Application
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
            // Act
            var result = _shippingService.GetShippingCost("uk");

            // Assert
            result.Amount.Amount.Should().Be(3m);
            result.CountryCode.Should().Be("UK");
        }
    }
}
