namespace Cavity.Collections
{
    using System;

    public class NormalityComparer : INormalityComparer
    {
        private static readonly NormalityComparer _currentCulture = new NormalityComparer
                                                                        {
                                                                            Comparison = StringComparison.CurrentCulture
                                                                        };

        private static readonly NormalityComparer _ordinal = new NormalityComparer
                                                                 {
                                                                     Comparison = StringComparison.Ordinal
                                                                 };

        private static readonly NormalityComparer _ordinalIgnoreCase = new NormalityComparer
                                                                           {
                                                                               Comparison = StringComparison.OrdinalIgnoreCase
                                                                           };

        public NormalityComparer()
        {
            Comparison = StringComparison.Ordinal;
        }

        public static NormalityComparer CurrentCulture
        {
            get
            {
                return _currentCulture;
            }
        }

        public static NormalityComparer Ordinal
        {
            get
            {
                return _ordinal;
            }
        }

        public static NormalityComparer OrdinalIgnoreCase
        {
            get
            {
                return _ordinalIgnoreCase;
            }
        }

        public StringComparison Comparison { get; set; }

        public virtual bool Equals(string x,
                                   string y)
        {
            return string.Equals(x, Normalize(y), Comparison);
        }

        public int GetHashCode(string obj)
        {
            return null == obj
                       ? 0
                       : Normalize(obj).GetHashCode();
        }

        public virtual string Normalize(string value)
        {
            if (null == value)
            {
                return null;
            }

#if NET20
            if (Comparison == StringComparison.OrdinalIgnoreCase ||
                Comparison == StringComparison.OrdinalIgnoreCase ||
                Comparison == StringComparison.InvariantCultureIgnoreCase)
#else
            if (Comparison.In(StringComparison.CurrentCultureIgnoreCase, StringComparison.OrdinalIgnoreCase, StringComparison.InvariantCultureIgnoreCase))
#endif
            {
                value = value.ToUpperInvariant();
            }

            return value;
        }
    }
}