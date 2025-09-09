using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Domain.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket> GetAsync(CancellationToken ct = default);
    }
}
