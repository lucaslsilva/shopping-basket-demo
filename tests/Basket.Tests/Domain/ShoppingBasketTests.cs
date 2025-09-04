using FluentAssertions;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Domain
{
    public class ShoppingBasketTests
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
    }
}
