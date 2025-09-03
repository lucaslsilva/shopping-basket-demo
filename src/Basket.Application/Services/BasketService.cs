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
            var item = new BaskedItem(request.ProductId, request.ProductName, new Money(request.UnitPrice, "GBP"), request.Quantity);
            basket.AddItem(item);
            return basket;
        }        
    }
}
