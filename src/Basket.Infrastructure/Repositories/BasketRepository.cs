using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Repositories;
using System.Collections.Concurrent;

namespace ShoppingBasket.Infrastructure.Repositories
{
    public sealed class BasketRepository : IBasketRepository
    {
        // In-memory storage for demonstration purposes
        private readonly Basket _basket = new();

        public Task<Basket> GetAsync(CancellationToken ct = default)
        {
            return Task.FromResult(_basket);
        }
    }
}
