namespace WhenFresh.Utilities.Collections;

public class SoundexComparer : INormalityComparer
{
    protected SoundexComparer()
    {
    }

    public static SoundexComparer Instance { get; } = new();

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
        return value.Soundex();
    }
}