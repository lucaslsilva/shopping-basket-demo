using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
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
    }
}
