using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IDiscountCodeService _discountCodeService;
        private readonly IShippingService _shippingService;

        public BasketService(
            IBasketRepository basketRepository, 
            IDiscountCodeService discountCodeService,
            IShippingService shippingService
        )
        {
            _basketRepository = basketRepository;
            _discountCodeService = discountCodeService;
            _shippingService = shippingService;
        }

        public Task<Basket> GetBasketAsync(CancellationToken ct = default)
        {
            return _basketRepository.GetAsync(ct);
        }

        public async Task<Basket> AddItemToBasketAsync(AddItemRequest request, CancellationToken ct = default)
        {
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
            var basket = await _basketRepository.GetAsync(ct);
            basket.RemoveItem(productId);
            return basket;
        }

        public async Task<Money> GetTotalWithoutVatAsync(CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetAsync(ct);
            return basket.GetTotalWithoutVat();
        }

        public async Task<Money> GetTotalWithVatAsync(CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetAsync(ct);
            return basket.GetTotalWithVat();
        }

        public async Task<Basket> ApplyDiscountCodeAsync(string code, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetAsync(ct);
            var discountCode = _discountCodeService.Validate(code);
            basket.ApplyDiscountCode(discountCode);
            return basket;
        }

        public async Task<Basket> SetShippingAsync(string countryCode, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetAsync(ct);
            var shippingCost = _shippingService.GetShippingCost(countryCode);
            basket.SetShippingCost(shippingCost);
            return basket;
        }

        public async Task<Basket> ClearBasketAsync(CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetAsync(ct);
            basket.Clear();
            return basket;
        }
    }
}
