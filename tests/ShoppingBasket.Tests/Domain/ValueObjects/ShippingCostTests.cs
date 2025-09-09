using FluentAssertions;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Domain.ValueObjects
{
    public class ShippingCostTests
    {
        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange & Act
            var amount = new Money(5m, "GBP");
            var shippingCost = new ShippingCost(amount, "UK");

            // Assert
            shippingCost.Amount.Should().Be(amount);
            shippingCost.CountryCode.Should().Be("UK");
        }
    }
}
