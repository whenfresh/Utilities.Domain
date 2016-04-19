namespace Cavity.Models
{
    using System;
    using System.Globalization;
    using Cavity.Xml.XPath;
    using Xunit;

    public sealed class CurrencyFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<Currency>()
                            .DerivesFrom<ValueObject<Currency>>()
                            .IsConcreteClass()
                            .IsSealed()
                            .HasDefaultConstructor()
                            .XmlRoot("currency")
                            .Result);
        }

        [Fact]
        public void ctor()
        {
            Assert.NotNull(new Currency());
        }

        [Fact]
        public void ctor_string_int()
        {
            Assert.NotNull(new Currency("€", 2));
        }

        [Fact]
        public void op_Parse_CultureInfoBritish_stringSterling()
        {
            var obj = new Currency("€", 2);

            var expected = new Money
                               {
                                   Amount = -123456.78m,
                                   Currency = obj
                               };

            var actual = obj.Parse(new CultureInfo("es-ES"), "-123.456,78 €");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Parse_CultureInfoNull_string()
        {
            var obj = new Currency("€", 2);

            Assert.Throws<ArgumentNullException>(() => obj.Parse(null, "-123.456,78 €"));
        }

        [Fact]
        public void op_Parse_CultureInfoSpanish_stringEuro()
        {
            var obj = new Currency("£", 2);

            var expected = new Money
                               {
                                   Amount = -123456.78m,
                                   Currency = obj
                               };

            var actual = obj.Parse(new CultureInfo("en-GB"), "-£123,456.78");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Parse_CultureInfo_stringEmpty()
        {
            var obj = new Currency("€", 2);

            Assert.Throws<ArgumentOutOfRangeException>(() => obj.Parse(new CultureInfo("en-GB"), string.Empty));
        }

        [Fact]
        public void op_Parse_CultureInfo_stringNull()
        {
            var obj = new Currency("€", 2);

            Assert.Throws<ArgumentNullException>(() => obj.Parse(new CultureInfo("en-GB"), null));
        }

        [Fact]
        public void op_ToNumberFormatInfo_CultureInfo()
        {
            var obj = new Currency("€", 2);

            var actual = obj.ToNumberFormatInfo(CultureInfo.CurrentUICulture);

            Assert.Equal(obj.Significance, actual.CurrencyDecimalDigits);
            Assert.Equal(".", actual.CurrencyDecimalSeparator);
            Assert.Equal(",", actual.CurrencyGroupSeparator);
            Assert.Equal(obj.Symbol, actual.CurrencySymbol);
        }

        [Fact]
        public void op_ToNumberFormatInfo_CultureInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Currency().ToNumberFormatInfo(null));
        }

        [Fact]
        public void prop_Code()
        {
            Assert.True(new PropertyExpectations<Currency>(x => x.Code)
                            .IsAutoProperty<string>()
                            .XmlAttribute("code")
                            .Result);
        }

        [Fact]
        public void prop_Number()
        {
            Assert.True(new PropertyExpectations<Currency>(x => x.Number)
                            .IsAutoProperty<string>()
                            .XmlAttribute("number")
                            .Result);
        }

        [Fact]
        public void prop_Significance()
        {
            Assert.True(new PropertyExpectations<Currency>(x => x.Significance)
                            .IsAutoProperty<int>()
                            .XmlAttribute("significance")
                            .Result);
        }

        [Fact]
        public void prop_Symbol()
        {
            Assert.True(new PropertyExpectations<Currency>(x => x.Symbol)
                            .IsAutoProperty<string>()
                            .XmlAttribute("symbol")
                            .Result);
        }

        [Fact]
        public void prop_Title()
        {
            Assert.True(new PropertyExpectations<Currency>(x => x.Title)
                            .IsAutoProperty<string>()
                            .XmlAttribute("title")
                            .Result);
        }

        [Fact]
        public void xml_deserialize()
        {
            var expected = new Currency
                               {
                                   Code = "EUR",
                                   Number = "978",
                                   Significance = 2,
                                   Symbol = "€",
                                   Title = "Euro"
                               };

            var actual = "<currency code='EUR' number='978' significance='2' symbol='€' title='Euro' />".XmlDeserialize<Currency>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void xml_deserialize_whenEmpty()
        {
            var expected = new Currency();
            var actual = "<currency />".XmlDeserialize<Currency>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void xml_serialize()
        {
            var obj = new Currency
                          {
                              Code = "EUR",
                              Number = "978",
                              Significance = 2,
                              Symbol = "€",
                              Title = "Euro"
                          };

            var navigator = obj.XmlSerialize().CreateNavigator();

            Assert.True(navigator.Evaluate<bool>("1=count(/currency[@code='EUR'][@number='978'][@significance='2'][@symbol='€'][@title='Euro'])"));
        }
    }
}