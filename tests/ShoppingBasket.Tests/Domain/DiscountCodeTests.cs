using FluentAssertions;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Tests.Domain
{
    public class DiscountCodeTests
    {
        [Theory]
        [InlineData(-5)]
        [InlineData(150)]
        public void DiscountCode_InvalidPercentage_ShouldThrow(decimal invalidPercentage)
        {
            Action act = () => new DiscountCode("SUMMER20", invalidPercentage);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void DiscountCode_InvalidCode_ShouldThrow(string invalidCode)
        {
            Action act = () => new DiscountCode(invalidCode!, 10);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void DiscountCode_Valid_ShouldSetProperties()
        {
            var code = new DiscountCode("SUMMER20", 20);
            code.Code.Should().Be("SUMMER20");
            code.Percentage.Should().Be(20);
        }
    }
}
