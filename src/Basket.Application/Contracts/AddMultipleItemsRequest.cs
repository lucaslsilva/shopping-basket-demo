namespace ShoppingBasket.Application.Contracts
{
    public record AddMultipleItemsRequest(IEnumerable<AddItemRequest> Items);
}
