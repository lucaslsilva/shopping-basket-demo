using FluentAssertions;
using Moq;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Application
{
    public class BasketServiceTests
    {
        private readonly Mock<IBasketRepository> _repositoryMock;
        private readonly Mock<IDiscountCodeService> _discountServiceMock;
        private readonly BasketService _basketService;

        public BasketServiceTests()
        {
            _repositoryMock = new();
            _discountServiceMock = new Mock<IDiscountCodeService>();
            _basketService = new BasketService(_repositoryMock.Object, _discountServiceMock.Object);
        }

        private void SetupRepositoryWithBasket(Basket basket)
        {
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(basket);
        }

        [Fact]
        public async Task AddItem_ShouldAddToBasket()
        {
            // Arrange
            var basket = new Basket();
            SetupRepositoryWithBasket(basket);

            var productId = Guid.NewGuid();
            var request = new AddItemRequest(productId, "Product", 10m, "GBP", 1);

            // Act
            var result = await _basketService.AddItemToBasketAsync(request);

            // Assert
            result.Items.Should().ContainSingle(i => i.ProductId == productId);
        }

        [Fact]
        public async Task AddMultipleItems_ShouldAddAllItems()
        {
            // Arrange
            var basket = new Basket();
            SetupRepositoryWithBasket(basket);

            var requests = new List<AddItemRequest>
            {
                new(Guid.NewGuid(), "Product 1", 10m, "GBP", 1),
                new(Guid.NewGuid(), "Product 2", 5m, "GBP", 2)
            };

            // Act
            var result = await _basketService.AddMultipleItemsToBasketAsync(new AddMultipleItemsRequest(requests));

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.First(i => i.ProductName == "Product 1").Quantity.Should().Be(1);
            result.Items.First(i => i.ProductName == "Product 2").Quantity.Should().Be(2);
        }

        [Fact]
        public async Task RemoveItem_ShouldRemoveItemFromBasket()
        {
            // Arrange
            var basket = new Basket();
            var itemId = Guid.NewGuid();
            basket.AddItem(new BasketItem(itemId, "Test Product", new Money(10, "GBP"), 2));
            SetupRepositoryWithBasket(basket);

            // Act
            var result = await _basketService.RemoveItemFromBasketAsync(itemId);

            // Assert
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBasket()
        {
            // Arrange
            var basket = new Basket();
            SetupRepositoryWithBasket(basket);

            // Act
            var result = await _basketService.GetBasketAsync();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ApplyDiscountCodeAsync_ValidCode_ShouldApplyDiscount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(100, "GBP"), 1));
            SetupRepositoryWithBasket(basket);

            // Mock discount service
            _discountServiceMock.Setup(s => s.Validate("SUMMER20"))
                                .Returns(new DiscountCode("SUMMER20", 20));

            // Act
            var result = await _basketService.ApplyDiscountCodeAsync("SUMMER20");

            // Assert
            result.DiscountCode.Should().NotBeNull();
            result.DiscountCode?.Code.Should().Be("SUMMER20");
            result.GetTotalWithoutVat().Amount.Should().Be(80);
        }

        [Fact]
        public async Task ApplyDiscountCodeAsync_InvalidCode_ShouldThrow()
        {
            // Arrange
            var basket = new Basket();
            SetupRepositoryWithBasket(basket);

            _discountServiceMock.Setup(s => s.Validate("INVALID"))
                                .Throws(new InvalidOperationException("Invalid discount code: INVALID"));

            // Ac
            Func<Task> act = async () => await _basketService.ApplyDiscountCodeAsync("INVALID");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Invalid discount code: INVALID");
        }
    }
}
