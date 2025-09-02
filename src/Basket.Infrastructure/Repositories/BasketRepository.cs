using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Repositories;
using System.Collections.Concurrent;

namespace ShoppingBasket.Infrastructure.Repositories
{
    public sealed class BasketRepository : IBasketRepository
    {
        // In-memory store for the baskets
        private readonly ConcurrentDictionary<Guid, Basket> _store = new();

        public Task<Basket> CreateAsync(CancellationToken ct = default)
        {
            var basket = new Basket();
            _store[basket.Id] = basket;
            return Task.FromResult(basket);
        }

        public Task<Basket?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return Task.FromResult(_store.GetValueOrDefault(id));
        }
    }
}
