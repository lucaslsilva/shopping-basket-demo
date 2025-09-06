using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Domain.Entities
{
    public sealed class Basket
    {
        private readonly List<BasketItem> _items = new();
        private string Currency => _items.FirstOrDefault()?.UnitPrice.Currency ?? "GBP"; // Default to GBP if no items

        public Guid Id { get; init; } = Guid.NewGuid();
        public DiscountCode? DiscountCode { get; private set; }
        public ShippingCost? ShippingCost { get; private set; }
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public void AddItem(BasketItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }
            // Ensure all items in the basket have the same currency
            if (_items.Count != 0 && item.UnitPrice.Currency != Currency)
            {
                throw new InvalidOperationException($"Cannot add item with currency {item.UnitPrice.Currency} to basket with currency {Currency}.");
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

        public void RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public Money GetTotalWithoutVat()
        {
            var totalAmount = _items.Sum(item => item.GetTotalPrice().Amount);

            if (DiscountCode is { } code)
            {
                var totalAmountEligibleForDiscount = _items
                    .Where(item => item.DiscountPercentage is null or 0)
                    .Sum(item => item.GetTotalPrice().Amount);

                var discountAmount = totalAmountEligibleForDiscount * (code.Percentage / 100);
                totalAmount -= discountAmount;
            }

            if (ShippingCost is { } shipping)
            {
                totalAmount += shipping.Amount.Amount;
            }

            return new Money(totalAmount, Currency);
        }

        public Money GetTotalWithVat(decimal vatRate = 0.2m)
        {
            if (vatRate < 0 || vatRate > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(vatRate), "VAT rate must be between 0 and 1.");
            }

            var totalWithoutVat = GetTotalWithoutVat();
            var vatAmount = totalWithoutVat * vatRate;
            return totalWithoutVat + vatAmount;
        }

        public void ApplyDiscountCode(DiscountCode discountCode)
        {
            DiscountCode = discountCode;
        }

        public void SetShippingCost(ShippingCost shippingCost)
        {
            ShippingCost = shippingCost;
        }

        public void Clear()
        {
            _items.Clear();
            DiscountCode = null;
            ShippingCost = null;
        }
    }
}
