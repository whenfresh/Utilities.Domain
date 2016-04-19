namespace Cavity.Models
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Cavity.Xml.XPath;
    using Xunit;

    public sealed class MoneyFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<Money>()
                            .IsValueType()
                            .XmlRoot("money")
                            .Implements<IEquatable<Money>>()
                            .Result);
        }

        [Fact]
        public void ctor()
        {
            Assert.NotNull(new Money());
        }

        [Fact]
        public void ctor_Currency_decimal()
        {
            Assert.NotNull(new Money(new Currency(), 1.23m));
        }

        [Fact]
        public void opEquality_Money_Money()
        {
            var euro = new Currency("€", 2);

            var obj = new Money(euro, 1.23m);
            var comparand = new Money(euro, 1.23m);

            Assert.True(obj == comparand);
        }

        [Fact]
        public void opInequality_Money_Money()
        {
            var obj = new Money(new Currency("€", 2), 1.23m);
            var comparand = new Money(new Currency("£", 2), 1.23m);

            Assert.True(obj != comparand);
        }

        [Fact]
        public void op_Equals_object()
        {
            var euro = new Currency("€", 2);

            var obj = new Money(euro, 1.23m);
            var comparand = new Money(euro, 1.23m);

            Assert.True(obj.Equals(comparand));
        }

        [Fact]
        public void op_Equals_objectDiffers()
        {
            var euro = new Currency("€", 2);

            var obj = new Money(euro, 1.23m);
            var comparand = new Money(euro, 4.56m);

            Assert.False(obj.Equals(comparand));
        }

        [Fact]
        public void op_Equals_objectInvalidCast()
        {
            var obj = new Uri("http://example.com/");

            Assert.Throws<InvalidCastException>(() => new Money().Equals(obj));
        }

        [Fact]
        public void op_Equals_objectNull()
        {
            Assert.False(new Money().Equals(null));
        }

        [Fact]
        public void op_GetHashCode()
        {
            const int expected = -1999077710;

            var actual = new Money(new Currency("€", 2), 1.23m).GetHashCode();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToString()
        {
            var culture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

                var obj = new Money(new Currency("€", 2), -123456.78m);

                const string expected = "-123.456,78 €";
                var actual = obj.ToString();

                Assert.Equal(expected, actual);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }

        [Fact]
        public void op_ToString_CultureInfo()
        {
            const string expected = "-£123,456.78";

            var actual = new Money(new Currency("£", 2), -123456.78m).ToString(new CultureInfo("en-GB"));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Amount()
        {
            Assert.True(new PropertyExpectations<Money>(x => x.Amount)
                            .IsAutoProperty<decimal>()
                            .XmlAttribute("amount")
                            .Result);
        }

        [Fact]
        public void prop_Currency()
        {
            Assert.True(new PropertyExpectations<Money>(x => x.Currency)
                            .IsAutoProperty<Currency>()
                            .XmlElement("currency")
                            .Result);
        }

        [Fact]
        public void xml_deserialize()
        {
            var expected = new Money(new Currency("€", 2), 1.23m);

            var actual = ("<money amount='1.23'>" +
                          "<currency significance='2' symbol='€' />" +
                          "</money>").XmlDeserialize<Money>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void xml_deserialize_whenEmpty()
        {
            var expected = new Money();
            var actual = "<money />".XmlDeserialize<Money>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void xml_serialize()
        {
            var obj = new Money(new Currency(), 1.23m);

            var navigator = obj.XmlSerialize().CreateNavigator();

            Assert.True(navigator.Evaluate<bool>("1=count(/money[@amount='1.23']/currency)"));
        }
    }
}