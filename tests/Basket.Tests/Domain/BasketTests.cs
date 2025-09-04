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
            var basket = new Basket();
            var price = new Money(10m, "GBP");

            basket.AddItem(new BasketItem(Guid.NewGuid(), "Test Product", price, 1));

            basket.Items.Should().HaveCount(1);
            basket.Items.First().ProductName.Should().Be("Test Product");
            basket.Items.First().Quantity.Should().Be(1);
            basket.Items.First().UnitPrice.Should().Be(price);
        }

        [Fact]
        public void AddItem_ShouldIncreaseQuantity_IfSameProduct()
        {
            var basket = new Basket();
            var price = new Money(10m, "GBP");
            var productId = Guid.NewGuid();

            basket.AddItem(new BasketItem(productId, "Test Product", price, 1));
            basket.AddItem(new BasketItem(productId, "Test Product", price, 2));

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
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(10m, "GBP"), 2)); // 20
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product B", new Money(5m, "GBP"), 1));  // 5

            var total = basket.GetTotalWithoutVat();

            total.Amount.Should().Be(25m);
        }

        [Fact]
        public void GetTotalWithVat_ShouldAdd20Percent()
        {
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(100m, "GBP"), 1)); // 100
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product B", new Money(50m, "GBP"), 2));  // 100

            var total = basket.GetTotalWithVat();

            total.Amount.Should().Be(240m); // 200 + 20%
        }
    }
}
