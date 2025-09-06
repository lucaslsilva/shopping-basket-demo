using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Application.Services
{
    public class DiscountCodeService : IDiscountCodeService
    {
        // Hardcoded valid discount codes for demonstration purposes
        private readonly IReadOnlyDictionary<string, decimal> _validCodes =
            new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["SUMMER20"] = 20,
                ["WELCOME10"] = 10
            };

        public DiscountCode Validate(string code)
        {
            if (!_validCodes.TryGetValue(code, out var percentage))
                throw new InvalidOperationException($"Invalid discount code: {code}");

            return new DiscountCode(code, percentage);
        }
    }
}
