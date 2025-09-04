using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Domain.Entities
{
    public sealed class BasketItem
    {
        public Guid ProductId { get; } = Guid.NewGuid();
        public string ProductName { get; }
        public Money UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal? DiscountPercentage { get; private set; } 

        public BasketItem(Guid productId, string productName, Money unitPrice, int quantity = 1, decimal? discountPercentage = null)
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
            if (DiscountPercentage is < 0 or > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(DiscountPercentage), "Discount percentage must be between 0 and 100.");
            }

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            DiscountPercentage = discountPercentage;
        }

        public void IncreaseQuantityBy(int valueToAdd)
        {
            if (valueToAdd <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valueToAdd), "Value to add must be at least 1.");
            }
            Quantity += valueToAdd;
        }

        public Money GetTotalPrice()
        {
            var totalAmount = UnitPrice.Amount * Quantity;
            if (DiscountPercentage.HasValue)
            {
                var discountAmount = totalAmount * (DiscountPercentage.Value / 100);
                totalAmount -= discountAmount;
            }
            return new(totalAmount, UnitPrice.Currency);
        }
    }
}
