namespace Cavity.Models
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public sealed class BritishTelephoneNumberLengthFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(typeof(BritishTelephoneNumberLength).IsStatic());
        }

        [Theory]
        [InlineData("(0800) 1111", true)]
        [InlineData("+2345678901", false)]
        [InlineData("+44500789012", true)]
        [InlineData("(0500) 789 012", true)]
        [InlineData("+445007890123", true)]
        [InlineData("(0500) 7890123", true)]
        [InlineData("+44800789012", true)]
        [InlineData("(0800) 789 012", true)]
        [InlineData("+448007890123", true)]
        [InlineData("(0800) 7890123", true)]
        [InlineData("+44120489012", true)]
        [InlineData("(01204) 89012", true)]
        [InlineData("+441204890123", true)]
        [InlineData("(01204) 890 123", true)]
        [InlineData("(01977) 123 45", false)]
        [InlineData("(01977) 123 456", true)]
        [InlineData("(0207) 1234 567", true)]
        public void op_Validate_Telephone(string telephone,
                                          bool expected)
        {
            var actual = BritishTelephoneNumberLength.Validate(telephone);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Validate_TelephoneEmpty()
        {
            Telephone telephone = string.Empty;

            Assert.Throws<ArgumentNullException>(() => BritishTelephoneNumberLength.Validate(telephone));
        }

        [Fact]
        public void op_Validate_TelephoneNull()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephoneNumberLength.Validate(null));
        }
    }
}