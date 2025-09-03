using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Domain.Entities
{
    public sealed class BasketItem
    {
        public Guid ProductId { get; } = Guid.NewGuid();
        public string ProductName { get; }
        public Money UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        public BasketItem(Guid productId, string productName, Money unitPrice, int quantity = 1)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
            }
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
            }
            if (unitPrice.Amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
            }
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");
            }

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public void IncreaseQuantityBy(int valueToAdd)
        {
            if (valueToAdd <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valueToAdd), "Value to add must be at least 1.");
            }
            Quantity += valueToAdd;
        }
    }
}
