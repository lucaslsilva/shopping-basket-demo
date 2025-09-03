using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Application.Contracts
{
    public record AddItemRequest(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity = 1
    );
}
