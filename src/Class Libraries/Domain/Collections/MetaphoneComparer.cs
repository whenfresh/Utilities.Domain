namespace Cavity.Collections
{
    using System;

    public class MetaphoneComparer : INormalityComparer
    {
        private static readonly MetaphoneComparer _instance = new MetaphoneComparer();

        protected MetaphoneComparer()
        {
        }

        public static MetaphoneComparer Instance
        {
            get
            {
                return _instance;
            }
        }

        public StringComparison Comparison { get; set; }

        public virtual bool Equals(string x,
                                   string y)
        {
            return x == Normalize(y);
        }

        public int GetHashCode(string obj)
        {
            return null == obj
                       ? 0
                       : Normalize(obj).GetHashCode();
        }

        public virtual string Normalize(string value)
        {
#if NET20
            return StringExtensionMethods.Metaphone(value);
#else
            return value.Metaphone();
#endif
        }
    }
}