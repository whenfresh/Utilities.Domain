﻿namespace WhenFresh.Utilities.Collections;

public sealed class UnderscoreComparer : NormalityComparer
{
    public override string Normalize(string value)
    {
        return value
               .ReplaceAllWith(string.Empty, StringComparison.Ordinal, "_")
               .ReplaceAllWith(string.Empty, StringComparison.Ordinal, "0");
    }
}