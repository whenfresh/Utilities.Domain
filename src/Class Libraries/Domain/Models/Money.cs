namespace Cavity.Models
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Xml.Serialization;

    [XmlRoot("money")]
    public struct Money : IEquatable<Money>
    {
        public Money(Currency currency,
                     decimal amount)
            : this()
        {
            Currency = currency;
            Amount = amount;
        }

        [XmlAttribute("amount")]
        public decimal Amount { get; set; }

        [XmlElement("currency")]
        public Currency Currency { get; set; }

        public static bool operator ==(Money obj,
                                       Money comparand)
        {
            return obj.Equals(comparand);
        }

        public static bool operator !=(Money obj,
                                       Money comparand)
        {
            return !obj.Equals(comparand);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            return Currency.GetHashCode() ^ Amount.GetHashCode();
        }

        public override string ToString()
        {
            return ToString(null, Currency);
        }

        public string ToString(CultureInfo culture)
        {
            return ToString(culture, Currency);
        }

        public bool Equals(Money other)
        {
            return Currency == other.Currency
                   && Amount == other.Amount;
        }

        private string ToString(CultureInfo culture,
                                Currency currency)
        {
            culture = culture ?? Thread.CurrentThread.CurrentUICulture;

            return string.Format(currency.ToNumberFormatInfo(culture), "{0:C}", Amount);
        }
    }
}