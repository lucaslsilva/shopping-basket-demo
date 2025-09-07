using FluentAssertions;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Domain.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Addition_WithSameCurrency_ShouldReturnCorrectSum()
        {
            // Arrange
            var m1 = new Money(50m, "GBP");
            var m2 = new Money(25m, "GBP");

            // Act
            var result = m1 + m2;

            // Assert
            result.Amount.Should().Be(75m);
            result.Currency.Should().Be("GBP");
        }

        [Fact]
        public void Addition_WithDifferentCurrencies_ShouldThrow()
        {
            // Arrange
            var m1 = new Money(50m, "GBP");
            var m2 = new Money(25m, "USD");

            // Act
            Action act = () => { var _ = m1 + m2; };

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Cannot add money with different currencies.");
        }

        [Fact]
        public void Subtraction_WithSameCurrency_ShouldReturnCorrectResult()
        {
            // Arrange
            var m1 = new Money(50m, "GBP");
            var m2 = new Money(20m, "GBP");

            // Act
            var result = m1 - m2;

            // Assert
            result.Amount.Should().Be(30m);
            result.Currency.Should().Be("GBP");
        }

        [Fact]
        public void Subtraction_WithDifferentCurrencies_ShouldThrow()
        {
            // Arrange
            var m1 = new Money(50m, "GBP");
            var m2 = new Money(20m, "USD");

            // Act
            Action act = () => { var _ = m1 - m2; };

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Cannot subtract money with different currencies.");
        }

        [Fact]
        public void Multiplication_ShouldReturnCorrectResult()
        {
            // Arrange
            var money = new Money(10m, "GBP");

            // Act
            var result = money * 3;

            // Assert
            result.Amount.Should().Be(30m);
            result.Currency.Should().Be("GBP");
        }

        [Fact]
        public void Division_ShouldReturnCorrectResult()
        {
            // Arrange
            var money = new Money(30m, "GBP");

            // Act
            var result = money / 3;

            // Assert
            result.Amount.Should().Be(10m);
            result.Currency.Should().Be("GBP");
        }

        [Fact]
        public void Division_ByZero_ShouldThrow()
        {
            // Arrange
            var money = new Money(30m, "GBP");

            // Act
            Action act = () => { var _ = money / 0; };

            // Assert
            act.Should().Throw<DivideByZeroException>()
               .WithMessage("Cannot divide by zero.");
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var money = new Money(123.45m, "GBP");

            // Act
            var str = money.ToString();

            // Assert
            str.Should().Be("123.45 GBP");
        }
    }
}
