namespace ShoppingBasket.Domain.Entities
{
    public sealed class Basket
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public List<BaskedItem> Items { get; init; } = new();
    }
}
