namespace WhenFresh.Utilities.Collections;

public interface INormalityComparer : IEqualityComparer<string>
{
    StringComparison Comparison { get; }

    string Normalize(string value);
}