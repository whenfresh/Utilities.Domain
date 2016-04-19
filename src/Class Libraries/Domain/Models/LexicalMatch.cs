namespace Cavity.Models
{
    using System;

    public sealed class LexicalMatch : ComparableObject
    {
        public LexicalMatch(LexicalItem item)
            : this()
        {
            if (null == item)
            {
                throw new ArgumentNullException("item");
            }

            Item = item;
        }

        private LexicalMatch()
        {
        }

        public LexicalItem Item { get; private set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public override string ToString()
        {
#if NET20
            return StringExtensionMethods.FormatWith("{0} {1} {2}", Prefix, Item, Suffix).Trim();
#else
            return "{0} {1} {2}".FormatWith(Prefix, Item, Suffix).Trim();
#endif
        }
    }
}