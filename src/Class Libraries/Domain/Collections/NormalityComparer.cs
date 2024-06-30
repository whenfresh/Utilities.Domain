namespace WhenFresh.Utilities.Collections;

public class NormalityComparer : INormalityComparer
{
    public NormalityComparer()
    {
        Comparison = StringComparison.Ordinal;
    }

    public static NormalityComparer CurrentCulture { get; } = new()
                                                                  {
                                                                      Comparison = StringComparison.CurrentCulture
                                                                  };

    public static NormalityComparer Ordinal { get; } = new()
                                                           {
                                                               Comparison = StringComparison.Ordinal
                                                           };

    public static NormalityComparer OrdinalIgnoreCase { get; } = new()
                                                                     {
                                                                         Comparison = StringComparison.OrdinalIgnoreCase
                                                                     };

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
            return null;

#if NET20
            if (Comparison == StringComparison.OrdinalIgnoreCase ||
                Comparison == StringComparison.OrdinalIgnoreCase ||
                Comparison == StringComparison.InvariantCultureIgnoreCase)
#else
        if (Comparison.In(StringComparison.CurrentCultureIgnoreCase, StringComparison.OrdinalIgnoreCase, StringComparison.InvariantCultureIgnoreCase))
#endif
            value = value.ToUpperInvariant();

        return value;
    }
}