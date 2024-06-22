namespace WhenFresh.Utilities.Domain.Facts.Collections
{
    using WhenFresh.Utilities.Core;

    public sealed class UnderscoreComparer : NormalityComparer
    {
        public override string Normalize(string value)
        {
            return value
                .ReplaceAllWith(string.Empty, StringComparison.Ordinal, "_")
                .ReplaceAllWith(string.Empty, StringComparison.Ordinal, "0");
        }
    }
}