namespace WhenFresh.Utilities.Collections;

public class CaverphoneComparer : INormalityComparer
{
    protected CaverphoneComparer()
    {
    }

    public static CaverphoneComparer Instance { get; } = new();

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
            return StringExtensionMethods.Caverphone(value);
#else
        return value.Caverphone();
#endif
    }
}