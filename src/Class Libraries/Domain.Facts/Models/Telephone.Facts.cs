namespace Cavity.Models
{
    using System;
    using Xunit;

    public sealed class TelephoneFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<Telephone>()
                            .DerivesFrom<ComparableObject>()
                            .IsConcreteClass()
                            .IsSealed()
                            .NoDefaultConstructor()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void opImplicit_Telephone_string()
        {
            const string value = "+441111222333";

            Telephone obj = value;

            Assert.Equal(value, obj.Number);
        }

        [Fact]
        public void opImplicit_Telephone_stringEmpty()
        {
            Telephone obj = string.Empty;

            Assert.Null(obj.Number);
        }

        [Fact]
        public void opImplicit_Telephone_stringNull()
        {
            Assert.Null((Telephone)null);
        }

        [Fact]
        public void op_FromString_string()
        {
            const string value = "+441111222333";

            var obj = Telephone.FromString(value);

            Assert.Equal(value, obj.Number);
        }

        [Fact]
        public void op_FromString_stringEmpty()
        {
            var expected = string.Empty;
            var actual = Telephone.FromString(expected).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_FromString_stringNull()
        {
            Assert.Throws<ArgumentNullException>(() => Telephone.FromString(null));
        }

        [Fact]
        public void op_FromString_string_whenDomesticFormat()
        {
            var obj = Telephone.FromString("(01111) 222-333");

            Assert.Equal("+441111222333", obj.Number);
        }

        [Fact]
        public void op_FromString_string_whenDomesticFormatAndComment()
        {
            var obj = Telephone.FromString("(01111) 222-333 at home");

            Assert.Equal("+441111222333", obj.Number);
        }

        [Fact]
        public void op_FromString_string_whenLongInternationalFormat()
        {
            var obj = Telephone.FromString("00441111222333");

            Assert.Equal("+441111222333", obj.Number);
        }

        [Fact]
        public void op_FromString_string_whenMissingLeadingZero()
        {
            var obj = Telephone.FromString("1111222333");

            Assert.Equal("+441111222333", obj.Number);
        }

        [Fact]
        public void op_FromString_string_whenNoDigits()
        {
            var obj = Telephone.FromString("not a number");

            Assert.Null(obj.Number);
        }

        [Fact]
        public void op_FromString_string_whenShortInternationalFormat()
        {
            var obj = Telephone.FromString("+1 (222) 333-4444");

            Assert.Equal("+12223334444", obj.Number);
        }

        [Fact]
        public void op_ToString()
        {
            const string expected = "+441111222333";
            var actual = Telephone.FromString(expected).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Number()
        {
            Assert.True(new PropertyExpectations<Telephone>(p => p.Number)
                            .TypeIs<string>()
                            .IsNotDecorated()
                            .Result);
        }
    }
}