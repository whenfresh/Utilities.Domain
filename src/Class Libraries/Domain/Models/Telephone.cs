namespace Cavity.Models
{
    using System;
    using System.Diagnostics;
#if !NET20
    using System.Linq;
#endif
    using Cavity.Diagnostics;

    public sealed class Telephone : ComparableObject
    {
        private Telephone()
        {
        }

        private Telephone(string number)
        {
            Number = number;
        }

        public string Number { get; set; }

        public static implicit operator Telephone(string value)
        {
            return ReferenceEquals(null, value)
                       ? null
                       : FromString(value);
        }

        public static Telephone FromString(string value)
        {
            Trace.WriteIf(Tracing.Is.TraceVerbose, value);
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Length)
            {
                return new Telephone();
            }

#if NET20
            var number = string.Empty;
            foreach (var c in value)
            {
                if (!char.IsDigit(c))
                {
                    continue;
                }

                number += c;
            }
#else
            var number = new string(value.AsEnumerable().Where(char.IsDigit).ToArray());
#endif

            if (2 > number.Length)
            {
                return new Telephone();
            }

            if ('+' == value[0])
            {
                return new Telephone("+" + number);
            }

            if (number.StartsWith("00", StringComparison.Ordinal))
            {
                return new Telephone("+" + number.Substring(2));
            }

            return '0' == number[0]
                       ? new Telephone("+44" + number.Substring(1))
                       : new Telephone("+44" + number);
        }

        public override string ToString()
        {
            return Number ?? string.Empty;
        }
    }
}