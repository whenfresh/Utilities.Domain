namespace Cavity.Collections
{
    using System;
    using System.Collections.Generic;

    public interface INormalityComparer : IEqualityComparer<string>
    {
        StringComparison Comparison { get; }

        string Normalize(string value);
    }
}