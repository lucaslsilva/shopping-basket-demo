namespace ShoppingBasket.Domain.ValueObjects
{
    public sealed record ShippingCost(Money Amount, string CountryCode);
}
