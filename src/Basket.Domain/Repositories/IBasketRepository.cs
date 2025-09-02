namespace Basket.Domain.Repositories
{
    public interface IBasketRepository
    {
        Task<Entities.Basket> CreateAsync(CancellationToken ct = default);
        Task<Entities.Basket?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
