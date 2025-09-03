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

            basket.AddItem(new BaskedItem(Guid.NewGuid(), "Test Product", price, 1));

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

            basket.AddItem(new BaskedItem(productId, "Test Product", price, 1));
            basket.AddItem(new BaskedItem(productId, "Test Product", price, 2));

            basket.Items.Should().HaveCount(1);
            basket.Items.First().Quantity.Should().Be(3);
        }
    }
}
