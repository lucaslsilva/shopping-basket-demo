namespace ShoppingBasket.Domain.Entities
{
    public sealed class Basket
    {
        private readonly List<BaskedItem> _items = new();

        public Guid Id { get; init; } = Guid.NewGuid();
        public IReadOnlyCollection<BaskedItem> Items => _items.AsReadOnly();

        public void AddItem(BaskedItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }

            var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                // If the item already exists, increase the quantity
                existingItem.IncreaseQuantityBy(item.Quantity);
            }
            else
            {
                _items.Add(item);
            }
        }
    }
}
