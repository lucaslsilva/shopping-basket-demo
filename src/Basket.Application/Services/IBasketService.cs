using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Domain.Entities;

namespace ShoppingBasket.Application.Services
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync(CancellationToken ct = default);
        Task AddItemToBasketAsync(AddItemRequest request, CancellationToken ct = default);
    }
}
