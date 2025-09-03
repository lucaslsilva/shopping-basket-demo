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
        public async Task GetAsync_ShouldReturnBasket()
        {
            var basket = await _service.GetBasketAsync();

            basket.Should().NotBeNull();
        }
    }
}
