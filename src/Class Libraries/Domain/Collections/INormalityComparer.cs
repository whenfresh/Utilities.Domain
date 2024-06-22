namespace WhenFresh.Utilities.Domain.Collections
{
    public interface INormalityComparer : IEqualityComparer<string>
    {
        StringComparison Comparison { get; }

        string Normalize(string value);
    }
}