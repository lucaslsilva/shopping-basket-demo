using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Domain.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket> CreateAsync(CancellationToken ct = default);
        Task<Basket?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
