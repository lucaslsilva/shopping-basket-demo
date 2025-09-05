using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Application.Contracts
{
    public record ApplyDiscountCodeRequest([Required] string Code);
}
