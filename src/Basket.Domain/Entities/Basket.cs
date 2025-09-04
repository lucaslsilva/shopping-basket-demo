using ShoppingBasket.Domain.ValueObjects;
using System;

namespace ShoppingBasket.Domain.Entities
{
    public sealed class Basket
    {
        private readonly List<BasketItem> _items = new();
        private string _currency => _items.FirstOrDefault()?.UnitPrice.Currency ?? "GBP"; // Default to GBP if no items

        public Guid Id { get; init; } = Guid.NewGuid();
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public void AddItem(BasketItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }
            // Ensure all items in the basket have the same currency
            if (item.UnitPrice.Currency != _currency)
            {
                throw new InvalidOperationException($"Cannot add item with currency {item.UnitPrice.Currency} to basket with currency {_currency}.");
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
            var totalAmount = _items.Sum(item => item.UnitPrice.Amount * item.Quantity);
            return new Money(totalAmount, _currency);
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
    }
}
