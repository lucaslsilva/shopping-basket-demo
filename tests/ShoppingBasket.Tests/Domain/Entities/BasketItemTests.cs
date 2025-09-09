using FluentAssertions;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Domain.Entities
{
    public class BasketItemTests
    {
        [Fact]
        public void Constructor_ShouldSetProperties_WhenValidArguments()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var price = new Money(10m, "GBP");

            // Act
            var item = new BasketItem(productId, "Product A", price, 2, 20);

            // Assert
            item.ProductId.Should().Be(productId);
            item.ProductName.Should().Be("Product A");
            item.UnitPrice.Should().Be(price);
            item.Quantity.Should().Be(2);
            item.DiscountPercentage.Should().Be(20);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenProductIdIsEmpty()
        {
            // Act
            Action act = () => new BasketItem(Guid.Empty, "Product", new Money(10m, "GBP"), 1);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Product ID cannot be empty. (Parameter 'productId')");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrow_WhenProductNameIsInvalid(string invalidName)
        {
            // Act
            Action act = () => new BasketItem(Guid.NewGuid(), invalidName, new Money(10m, "GBP"), 1);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Product name cannot be null or empty. (Parameter 'productName')");
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenUnitPriceIsNegative()
        {
            // Act
            Action act = () => new BasketItem(Guid.NewGuid(), "Product", new Money(-1m, "GBP"), 1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("Unit price cannot be negative. (Parameter 'unitPrice')");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Constructor_ShouldThrow_WhenQuantityIsZeroOrNegative(int invalidQuantity)
        {
            // Act
            Action act = () => new BasketItem(Guid.NewGuid(), "Product", new Money(10m, "GBP"), invalidQuantity);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("Quantity must be at least 1. (Parameter 'quantity')");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void Constructor_ShouldThrow_WhenDiscountPercentageIsOutOfRange(decimal invalidDiscount)
        {
            // Act
            Action act = () => new BasketItem(Guid.NewGuid(), "Product", new Money(10m, "GBP"), 1, invalidDiscount);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("Discount percentage must be between 0 and 100. (Parameter 'DiscountPercentage')");
        }

        [Fact]
        public void IncreaseQuantityBy_ShouldIncreaseQuantity()
        {
            // Arrange
            var item = new BasketItem(Guid.NewGuid(), "Product", new Money(10m, "GBP"), 1);

            // Act
            item.IncreaseQuantityBy(3);

            // Assert
            item.Quantity.Should().Be(4);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void IncreaseQuantityBy_ShouldThrow_WhenValueIsZeroOrNegative(int invalidValue)
        {
            // Arrange
            var item = new BasketItem(Guid.NewGuid(), "Product", new Money(10m, "GBP"), 1);

            // Act
            Action act = () => item.IncreaseQuantityBy(invalidValue);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("Value to add must be at least 1. (Parameter 'valueToAdd')");
        }

        [Fact]
        public void GetTotalPrice_ShouldReturnCorrectValue_WithoutDiscount()
        {
            // Arrange
            var item = new BasketItem(Guid.NewGuid(), "Product", new Money(10m, "GBP"), 3);

            // Act
            var total = item.GetTotalPrice();

            // Assert
            total.Amount.Should().Be(30m);
            total.Currency.Should().Be("GBP");
        }

        [Fact]
        public void GetTotalPrice_ShouldApplyDiscount_WhenDiscountExists()
        {
            // Arrange
            var item = new BasketItem(Guid.NewGuid(), "Product", new Money(100m, "GBP"), 2, 25);

            // Act
            var total = item.GetTotalPrice();

            // Assert
            total.Amount.Should().Be(150m); // (200 - 25%)
            total.Currency.Should().Be("GBP");
        }
    }
}
