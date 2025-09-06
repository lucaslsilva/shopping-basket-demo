using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Application.Services
{
    public class ShippingService : IShippingService
    {
        public ShippingCost GetShippingCost(string countryCode)
        {
            return countryCode.ToUpper() switch
            {
                "US" => new ShippingCost(new Money(5.00m, "GBP"), "US"),
                "UK" => new ShippingCost(new Money(3.00m, "GBP"), "UK"),
                "DE" => new ShippingCost(new Money(4.00m, "GBP"), "DE"),
                _ => new ShippingCost(new Money(10.00m, "GBP"), countryCode.ToUpper())
            };
        }
    }
}
