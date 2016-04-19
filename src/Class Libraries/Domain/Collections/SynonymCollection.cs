namespace Cavity.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

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

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        protected INormalityComparer Comparer
        {
            get
            {
                return _comparer;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                _comparer = value;
            }
        }

        protected Dictionary<string, string> Items { get; private set; }

        public virtual void Add(string value)
        {
            value = Comparer.Normalize(value);
            if (Contains(value))
            {
                return;
            }

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IEnumerator<string> GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }
    }
}