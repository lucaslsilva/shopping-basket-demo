namespace ShoppingBasket.Domain.ValueObjects
{
    public readonly record struct DiscountCode
    {
        public string Code { get; init; }
        public decimal Percentage { get; init; }

        public DiscountCode(string code, decimal percentage)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code cannot be null or empty", nameof(code));
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100");

            Code = code;
            Percentage = percentage;
        }
    }
}
