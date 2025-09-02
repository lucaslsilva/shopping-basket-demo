using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Domain.Entities
{
    public sealed class BaskedItem
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; }
        public Money UnitPrice { get; init; }
        public int Quantity { get; set; }
        public BaskedItem(Guid id, string name, Money unitPrice, int quantity)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
