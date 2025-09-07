using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Application.Services
{
    public class BasketServiceTests
    {
        private readonly Mock<IBasketRepository> _repositoryMock;
        private readonly Mock<IDiscountCodeService> _discountServiceMock;
        private readonly Mock<IShippingService> _shippingServiceMock;
        private readonly Mock<ILogger<BasketService>> _loggerMock;
        private readonly BasketService _basketService;

        public BasketServiceTests()
        {
            _repositoryMock = new Mock<IBasketRepository>();
            _discountServiceMock = new Mock<IDiscountCodeService>();
            _shippingServiceMock = new Mock<IShippingService>();
            _loggerMock = new Mock<ILogger<BasketService>>();

            _basketService = new BasketService(
                _repositoryMock.Object,
                _discountServiceMock.Object,
                _shippingServiceMock.Object,
                _loggerMock.Object
            );
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
        public async Task AddItem_WhenBasketNotFound_ShouldThrow()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Basket?)null);

            var request = new AddItemRequest(Guid.NewGuid(), "Product", 10m, "GBP", 1);

            // Act
            Func<Task> act = async () => await _basketService.AddItemToBasketAsync(request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Basket not found");
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
        public async Task GetTotalWithoutVatAsync_ShouldReturnCorrectTotal()
        {
            // Arrange
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(100, "GBP"), 1));
            SetupRepositoryWithBasket(basket);

            // Act
            var total = await _basketService.GetTotalWithoutVatAsync();

            // Assert
            total.Amount.Should().Be(100m);
            total.Currency.Should().Be("GBP");
        }

        [Fact]
        public async Task GetTotalWithVatAsync_ShouldReturnCorrectTotal()
        {
            // Arrange
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(100, "GBP"), 1));
            SetupRepositoryWithBasket(basket);

            // Act
            var total = await _basketService.GetTotalWithVatAsync();

            // Assert
            total.Amount.Should().Be(120m); // 100 + 20% VAT
            total.Currency.Should().Be("GBP");
        }

        [Fact]
        public async Task ApplyDiscountCodeAsync_ValidCode_ShouldApplyDiscount()
        {
            // Arrange
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(100, "GBP"), 1));
            SetupRepositoryWithBasket(basket);

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

            // Act
            Func<Task> act = async () => await _basketService.ApplyDiscountCodeAsync("INVALID");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Invalid discount code: INVALID");
        }

        [Theory]
        [InlineData("UK", 3)]
        [InlineData("US", 5)]
        [InlineData("DE", 4)]
        [InlineData("FR", 10)]
        public async Task SetShipping_ShouldApplyCorrectCost(string countryCode, decimal expectedAmount)
        {
            // Arrange
            var basket = new Basket();
            SetupRepositoryWithBasket(basket);

            _shippingServiceMock.Setup(s => s.GetShippingCost(It.IsAny<string>()))
                                .Returns((string cc) =>
                                    new ShippingCost(new Money(
                                        cc.ToUpper() switch
                                        {
                                            "UK" => 3m,
                                            "US" => 5m,
                                            "DE" => 4m,
                                            _ => 10m
                                        }, "GBP"), cc.ToUpper())
                                );

            // Act
            var result = await _basketService.SetShippingAsync(countryCode);

            // Assert
            result.ShippingCost.Should().NotBeNull();
            result.ShippingCost?.Amount.Amount.Should().Be(expectedAmount);
            result.ShippingCost?.CountryCode.Should().Be(countryCode.ToUpper());
        }

        [Fact]
        public async Task ClearBasket_ShouldRemoveAllItemsAndDiscounts()
        {
            // Arrange
            var basket = new Basket();
            basket.AddItem(new BasketItem(Guid.NewGuid(), "Product A", new Money(10m, "GBP"), 1));
            basket.SetShippingCost(new ShippingCost(new Money(5m, "GBP"), "UK"));
            basket.ApplyDiscountCode(new DiscountCode("SUMMER20", 20));
            SetupRepositoryWithBasket(basket);

            // Act
            var result = await _basketService.ClearBasketAsync();

            // Assert
            result.Items.Should().BeEmpty();
            result.ShippingCost.Should().BeNull();
            result.DiscountCode.Should().BeNull();
        }
    }
}
