namespace Cavity.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
#if !NET20
    using System.Linq;
#endif
    using Cavity.Models;

    public class LexicalCollection : IEnumerable<LexicalItem>
    {
        private readonly List<LexicalItem> _items;

        private INormalityComparer _comparer;

        public LexicalCollection(INormalityComparer comparer)
            : this()
        {
            Comparer = comparer;
        }

        private LexicalCollection()
        {
            _items = new List<LexicalItem>();
        }

        public IEnumerable<string> CanonicalForms
        {
            get
            {
#if NET20
                foreach (var item in this)
                {
                    yield return item.CanonicalForm;
                }
#else
                return this.Select(item => item.CanonicalForm);
#endif
            }
        }

        public INormalityComparer Comparer
        {
            get
            {
                return _comparer;
            }

            private set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                _comparer = value;
            }
        }

        public virtual LexicalItem this[string spelling]
        {
            get
            {
                if (null == spelling)
                {
                    throw new ArgumentNullException("spelling");
                }

#if NET20
                foreach (var item in this)
                {
                    if (item.Contains(spelling))
                    {
                        return item;
                    }
                }

                return null;
#else
                return this.FirstOrDefault(item => item.Contains(spelling));
#endif
            }
        }

        public virtual LexicalItem Add(LexicalItem item)
        {
            if (null == item)
            {
                throw new ArgumentNullException("item");
            }

            var copy = Add(item.CanonicalForm);
            foreach (var synonym in item.Synonyms)
            {
                copy.Synonyms.Add(synonym);
            }

            return copy;
        }

        public virtual LexicalItem Add(string value)
        {
            var item = this[value];
            if (null != item)
            {
                return item;
            }

            item = new LexicalItem(Comparer, value);

            _items.Add(item);

            return item;
        }

        public virtual bool Contains(string value)
        {
#if NET20
            foreach (var item in this)
            {
                if (item.Contains(value))
                {
                    return true;
                }
            }

            return false;
#else
            return this.Any(item => item.Contains(value));
#endif
        }

#if !NET20
        public virtual void Invoke(Func<string, string> func)
        {
            if (null == func)
            {
                throw new ArgumentNullException("func");
            }

            foreach (var item in this)
            {
                item.Invoke(func);
            }
        }

#endif

        public LexicalMatch Match(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Trim().Length)
            {
                return null;
            }

#if NET20
            foreach (var item in this)
            {
                var result = item.Match(value);
                if (null == result)
                {
                    continue;
                }

                return result;
            }

            return null;
#else
            return this
                .Select(item => item.Match(value))
                .FirstOrDefault(result => null != result);
#endif
        }

        public LexicalMatch MatchBeginning(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Trim().Length)
            {
                return null;
            }

#if NET20
            foreach (var item in this)
            {
                var result = item.MatchBeginning(value);
                if (null == result)
                {
                    continue;
                }

                return result;
            }

            return null;
#else
            return this
                .OrderByDescending(x => x.CanonicalForm.Length)
                .Select(item => item.MatchBeginning(value))
                .FirstOrDefault(result => null != result);
#endif
        }

        public LexicalMatch MatchEnding(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Trim().Length)
            {
                return null;
            }

#if NET20
            foreach (var item in this)
            {
                var result = item.MatchEnding(value);
                if (null == result)
                {
                    continue;
                }

                return result;
            }

            return null;
#else
            return this
                .OrderByDescending(x => x.CanonicalForm.Length)
                .Select(item => item.MatchEnding(value))
                .FirstOrDefault(result => null != result);
#endif
        }

#if !NET20
        public LexicalMatch MatchWithin(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Trim().Length)
            {
                return null;
            }

            return this
                .OrderByDescending(x => x.CanonicalForm.Length)
                .Select(item => item.MatchWithin(value))
                .FirstOrDefault(result => null != result);
        }

#endif

        public virtual void MoveTo(LexicalCollection destination,
                                   LexicalItem item)
        {
            if (null == destination)
            {
                throw new ArgumentNullException("destination");
            }

            if (ReferenceEquals(this, destination))
            {
                throw new InvalidOperationException("The source and destination cannot be the same instance.");
            }

            if (null == item)
            {
                throw new ArgumentNullException("item");
            }

            Remove(new[] { item });
            destination.Add(item);
        }

        public virtual void Remove(LexicalItem item)
        {
            if (null == item)
            {
                throw new ArgumentNullException("item");
            }

            Remove(new[] { item });
        }

        public virtual void Remove(IEnumerable<LexicalItem> items)
        {
            if (null == items)
            {
                throw new ArgumentNullException("items");
            }

#if NET20
            foreach (var item in items)
            {
                foreach (var spelling in item.Spellings)
                {
                    var match = this[spelling];
                    if (null == match)
                    {
                        continue;
                    }

                    _items.Remove(match);
                }
            }
#else
            foreach (var match in items.SelectMany(item => item.Spellings
                                                               .Select(x => this[x])
                                                               .Where(match => null != match)))
            {
                _items.Remove(match);
            }

#endif
        }

        public IEnumerator<LexicalItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public virtual string ToCanonicalForm(string value)
        {
#if NET20
            foreach (var item in this)
            {
                if (!item.Contains(value))
                {
                    continue;
                }

                return item.CanonicalForm;
            }

            return null;
#else
            return (from item in this
                    where item.Contains(value)
                    select item.CanonicalForm).FirstOrDefault();
#endif
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}