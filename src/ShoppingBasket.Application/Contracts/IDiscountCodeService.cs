using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Application.Services
{
    public interface IDiscountCodeService
    {
        DiscountCode Validate(string code);
    }
}
