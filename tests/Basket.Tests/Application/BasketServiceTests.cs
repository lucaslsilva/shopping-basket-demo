using FluentAssertions;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Domain.ValueObjects;
using ShoppingBasket.Infrastructure.Repositories;

namespace ShoppingBasket.Tests.Application
{
    public class BasketServiceTests
    {
        private readonly IBasketRepository _repo;
        private readonly BasketService _service;

        public BasketServiceTests()
        {
            _repo = new BasketRepository();
            _service = new BasketService(_repo);
        }

        [Fact]
        public async Task AddItem_ShouldAddToBasket()
        {
            var productId = Guid.NewGuid();
            var request = new AddItemRequest(productId, "Product", 10m, "GBP", 1);

            var basket = await _service.AddItemToBasketAsync(request);

            basket.Items.Should().ContainSingle(i => i.ProductId == productId);
        }

        [Fact]
        public async Task AddMultipleItems_ShouldAddAllItems()
        {
            var requests = new List<AddItemRequest>
            {
                new(Guid.NewGuid(), "Product 1", 10m, "GBP", 1),
                new(Guid.NewGuid(), "Product 2", 5m, "GBP", 2)
            };

            var basket = await _service.AddMultipleItemsToBasketAsync(new AddMultipleItemsRequest(requests));

            basket.Items.Should().HaveCount(2);
            basket.Items.First(i => i.ProductName == "Product 1").Quantity.Should().Be(1);
            basket.Items.First(i => i.ProductName == "Product 2").Quantity.Should().Be(2);
            basket.Items.All(i => i.UnitPrice.Amount > 0).Should().BeTrue();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBasket()
        {
            var basket = await _service.GetBasketAsync();

            basket.Should().NotBeNull();
        }
    }
}
