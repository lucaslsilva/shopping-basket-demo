using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace ShoppingBasket.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IDiscountCodeService _discountCodeService;
        private readonly IShippingService _shippingService;
        private readonly ILogger<BasketService> _logger;

        public BasketService(
            IBasketRepository basketRepository, 
            IDiscountCodeService discountCodeService,
            IShippingService shippingService,
            ILogger<BasketService> logger
        )
        {
            _basketRepository = basketRepository;
            _discountCodeService = discountCodeService;
            _shippingService = shippingService;
            _logger = logger;
        }

        public Task<Basket> GetBasketAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Retrieving basket");

            return _basketRepository.GetAsync(ct);
        }

        public async Task<Basket> AddItemToBasketAsync(AddItemRequest request, CancellationToken ct = default)
        {
            _logger.LogInformation("Adding item {ProductName} ({ProductId}) with quantity {Quantity}",
                request.ProductName, request.ProductId, request.Quantity);

            var basket = await _basketRepository.GetAsync(ct) ?? throw new InvalidOperationException("Basket not found");
            var item = new BasketItem(
                request.ProductId, 
                request.ProductName, 
                new Money(request.UnitPrice, request.Currency), 
                request.Quantity, 
                request.DiscountPercentage
            );
            basket.AddItem(item);
            return basket;
        }

        public async Task<Basket> AddMultipleItemsToBasketAsync(AddMultipleItemsRequest request, CancellationToken ct = default)
        {
            _logger.LogInformation("Adding multiple items to basket: {ItemCount} items", request.Items.Count());

            var basket = await _basketRepository.GetAsync(ct);

            foreach (var item in request.Items)
            {
                var basketItem = new BasketItem(
                    item.ProductId,
                    item.ProductName,
                    new Money(item.UnitPrice, item.Currency),
                    item.Quantity
                );

                basket.AddItem(basketItem);
            }

            return basket;
        }

        public async Task<Basket> RemoveItemFromBasketAsync(Guid productId, CancellationToken ct = default)
        {
            _logger.LogInformation("Removing item with ProductId {ProductId} from basket", productId);

            var basket = await _basketRepository.GetAsync(ct);
            basket.RemoveItem(productId);
            return basket;
        }

        public async Task<Money> GetTotalWithoutVatAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Calculating total without VAT");

            var basket = await _basketRepository.GetAsync(ct);
            return basket.GetTotalWithoutVat();
        }

        public async Task<Money> GetTotalWithVatAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Calculating total with VAT");

            var basket = await _basketRepository.GetAsync(ct);
            return basket.GetTotalWithVat();
        }

        public async Task<Basket> ApplyDiscountCodeAsync(string code, CancellationToken ct = default)
        {
            _logger.LogInformation("Applying discount code {DiscountCode}", code);

            var basket = await _basketRepository.GetAsync(ct);
            var discountCode = _discountCodeService.Validate(code);
            basket.ApplyDiscountCode(discountCode);
            return basket;
        }

        public async Task<Basket> SetShippingAsync(string countryCode, CancellationToken ct = default)
        {
            _logger.LogInformation("Setting shipping for country code {CountryCode}", countryCode);

            var basket = await _basketRepository.GetAsync(ct);
            var shippingCost = _shippingService.GetShippingCost(countryCode);
            basket.SetShippingCost(shippingCost);
            return basket;
        }

        public async Task<Basket> ClearBasketAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Clearing the basket");

            var basket = await _basketRepository.GetAsync(ct);
            basket.Clear();
            return basket;
        }
    }
}
