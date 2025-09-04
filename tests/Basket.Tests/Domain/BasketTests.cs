using FluentAssertions;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Domain
{
    public class BasketTests
    {
        [Fact]
        public void AddItem_ShouldAddNewItem()
        {
            // Arrange
            var basket = new Basket();
            var price = new Money(10m, "GBP");

            // Act
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Test Product", price, 1));

            // Assert
            basket.Items.Should().HaveCount(1);
            basket.Items.First().ProductName.Should().Be("Test Product");
            basket.Items.First().Quantity.Should().Be(1);
            basket.Items.First().UnitPrice.Should().Be(price);
        }

        [Fact]
        public void AddItem_ShouldIncreaseQuantity_IfSameProduct()
        {
            // Arrange
            var basket = new Basket();
            var price = new Money(10m, "GBP");
            var productId = Guid.NewGuid();

            // Act
            basket.AddItem(new BasketItem(productId, "Test Product", price, 1));
            basket.AddItem(new BasketItem(productId, "Test Product", price, 2));

            // Assert
            basket.Items.Should().HaveCount(1);
            basket.Items.First().Quantity.Should().Be(3);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItemFromBasket()
        {
            // Arrange
            var basket = new Basket();
            var itemId = Guid.NewGuid();
            var item = new BasketItem(itemId, "Test Product", new Money(10m, "GBP"), 2);
            basket.AddItem(item);

            // Act
            basket.RemoveItem(itemId);

            // Assert
            basket.Items.Should().BeEmpty();
        }

        [Fact]
        public void AddItem_SameProduct_ShouldIncreaseQuantity()
        {
            // Arrange
            var basket = new Basket();
            var productId = Guid.NewGuid();
            var item1 = new BasketItem(productId, "Test Product", new Money(10m, "GBP"), 1);
            var item2 = new BasketItem(productId, "Test Product", new Money(10m, "GBP"), 2);

            // Act
            basket.AddItem(item1);
            basket.AddItem(item2);

            // Assert
            basket.Items.Should().HaveCount(1);
            basket.Items.First().Quantity.Should().Be(3);
        }

        [Fact]
        public void AddItem_DifferentProducts_ShouldAddSeparately()
        {
            // Arrange
            var basket = new Basket();
            var item1 = new BasketItem(Guid.NewGuid(), "Product A", new Money(5m, "GBP"), 1);
            var item2 = new BasketItem(Guid.NewGuid(), "Product B", new Money(15m, "GBP"), 1);

            // Act
            basket.AddItem(item1);
            basket.AddItem(item2);

            // Assert
            basket.Items.Should().HaveCount(2);
        }

        [Fact]
        public void AddItem_WithDifferentCurrency_ShouldThrow()
        {
            // Arrange
            var basket = new Basket();
            var item1 = new BasketItem(Guid.NewGuid(), "Product A", new Money(10m, "GBP"), 1);
            var item2 = new BasketItem(Guid.NewGuid(), "Product B", new Money(15m, "USD"), 1);

            basket.AddItem(item1);

            // Act
            Action act = () => basket.AddItem(item2);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Cannot add item with currency USD to basket with currency GBP.");
        }

        [Fact]
        public void GetTotalWithoutVat_ShouldReturnCorrectSum()
        {
            // Arrange
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(10m, "GBP"), 2)); // 20
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product B", new Money(5m, "GBP"), 1));  // 5

            // Act
            var total = basket.GetTotalWithoutVat();

            // Assert
            total.Amount.Should().Be(25m);
        }

        [Fact]
        public void GetTotalWithVat_ShouldAdd20Percent()
        {
            // Arrange
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(100m, "GBP"), 1)); // 100
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product B", new Money(50m, "GBP"), 2));  // 100

            // Act
            var total = basket.GetTotalWithVat();

            // Assert
            total.Amount.Should().Be(240m); // 200 + 20%
        }

        [Fact]
        public void AddItem_WithDiscount_ShouldApplyDiscountToTotal()
        {
            // Arrange
            var basket = new Basket();
            var discountedItem = new BasketItem(
                Guid.NewGuid(),
                "Product A",
                new Money(100m, "GBP"),
                quantity: 2,
                discountPercentage: 25);

            // Act
            basket.AddItem(discountedItem);

            // Assert
            var totalWithoutVat = basket.GetTotalWithoutVat();
            totalWithoutVat.Amount.Should().Be(150m); // (100 * 0.75) * 2
            totalWithoutVat.Currency.Should().Be("GBP");
        }
    }
}
