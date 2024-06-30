namespace WhenFresh.Utilities.Collections;

public class SynonymCollection : IEnumerable<string>
{
    private INormalityComparer _comparer;

    public SynonymCollection(INormalityComparer comparer)
        : this()
    {
        Comparer = comparer;
        Items = new Dictionary<string, string>(comparer);
    }

    private SynonymCollection()
    {
    }

    public int Count => Items.Count;

    protected INormalityComparer Comparer
    {
        get => _comparer;

        set
        {
            if (null == value)
                throw new ArgumentNullException("value");

            _comparer = value;
        }
    }

    protected Dictionary<string, string> Items { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual IEnumerator<string> GetEnumerator()
    {
        return Items.Values.GetEnumerator();
    }

    public virtual void Add(string value)
    {
        value = Comparer.Normalize(value);
        if (Contains(value))
            return;

        Items.Add(value, value);
    }

    public virtual void Clear()
    {
        Items.Clear();
    }

    public virtual bool Contains(string value)
    {
        return Items.ContainsKey(value);
    }
}