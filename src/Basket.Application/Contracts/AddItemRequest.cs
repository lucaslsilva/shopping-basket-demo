namespace ShoppingBasket.Application.Contracts
{
    public record AddItemRequest(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        string Currency = "GBP",
        int Quantity = 1
    );
}
