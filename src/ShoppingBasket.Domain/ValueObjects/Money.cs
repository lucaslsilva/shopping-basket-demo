namespace ShoppingBasket.Domain.ValueObjects
{
    public readonly record struct Money(decimal Amount, string Currency)
    {
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
            {
                throw new InvalidOperationException("Cannot add money with different currencies.");
            }
            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
            {
                throw new InvalidOperationException("Cannot subtract money with different currencies.");
            }
            return new Money(a.Amount - b.Amount, a.Currency);
        }

        public static Money operator *(Money a, decimal factor)
        {
            return new Money(a.Amount * factor, a.Currency);
        }

        public static Money operator /(Money a, decimal divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            return new Money(a.Amount / divisor, a.Currency);
        }

        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }
    }
}
