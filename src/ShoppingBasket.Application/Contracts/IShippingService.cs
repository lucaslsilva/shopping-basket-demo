using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Application.Services
{
    public interface IShippingService
    {
        ShippingCost GetShippingCost(string countryCode);
    }
}
