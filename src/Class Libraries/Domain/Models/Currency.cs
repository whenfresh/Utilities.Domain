namespace Cavity.Models
{
    using System;
    using System.Globalization;
    using System.Xml.Serialization;

    [Comment("http://en.wikipedia.org/wiki/ISO_4217")]
    [XmlRoot("currency")]
    public sealed class Currency : ValueObject<Currency>
    {
        public Currency()
        {
            RegisterProperty(x => Code);
            RegisterProperty(x => Number);
            RegisterProperty(x => Significance);
            RegisterProperty(x => Symbol);
        }

        public Currency(string symbol,
                        int significance)
            : this()
        {
            Symbol = symbol;
            Significance = significance;
        }

        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlAttribute("number")]
        public string Number { get; set; }

        [XmlAttribute("significance")]
        public int Significance { get; set; }

        [XmlAttribute("symbol")]
        public string Symbol { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        public Money Parse(CultureInfo culture,
                           string value)
        {
            if (null == culture)
            {
                throw new ArgumentNullException("culture");
            }

            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Length)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return new Money
                       {
                           Amount = (decimal)double.Parse(value, NumberStyles.Currency, ToNumberFormatInfo(culture)),
                           Currency = this
                       };
        }

        public NumberFormatInfo ToNumberFormatInfo(CultureInfo culture)
        {
            if (null == culture)
            {
                throw new ArgumentNullException("culture");
            }

            var result = (NumberFormatInfo)culture.NumberFormat.Clone();
            result.CurrencyDecimalDigits = Significance;
            result.CurrencySymbol = Symbol;

            return result;
        }
    }
}