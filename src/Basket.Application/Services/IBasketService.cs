using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Application.Services
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync(CancellationToken ct = default);
        Task<Basket> AddItemToBasketAsync(AddItemRequest request, CancellationToken ct = default);
        Task<Basket> AddMultipleItemsToBasketAsync(AddMultipleItemsRequest request, CancellationToken ct = default);
        Task<Basket> RemoveItemFromBasketAsync(Guid productId, CancellationToken ct = default);
        Task<Money> GetTotalWithoutVatAsync(CancellationToken ct = default);
        Task<Money> GetTotalWithVatAsync(CancellationToken ct = default);
        Task<Basket> ApplyDiscountCodeAsync(string code, CancellationToken ct = default);
    }
}
