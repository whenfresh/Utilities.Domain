namespace Cavity.Models
{
    using System;
    using System.Collections.Generic;
#if !NET20
    using System.Linq;
#endif
    using Cavity.Collections;
    using Cavity.Data;

    public class LexicalItem
    {
        private KeyStringPair _canonicalForm;

        private INormalityComparer _comparer;

        public LexicalItem(INormalityComparer comparer,
                           string canonicalForm)
            : this()
        {
            Comparer = comparer;
            Synonyms = new SynonymCollection(comparer);
            CanonicalForm = canonicalForm;
        }

        private LexicalItem()
        {
        }

        public string CanonicalForm
        {
            get
            {
                return _canonicalForm.Value;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                value = value.Trim();
                if (0 == value.Length)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _canonicalForm = new KeyStringPair(Comparer.Normalize(value), value);
            }
        }

        public IEnumerable<string> Spellings
        {
            get
            {
                if (null != CanonicalForm)
                {
                    yield return CanonicalForm;
                }

                foreach (var synonym in Synonyms)
                {
                    yield return synonym;
                }
            }
        }

        public SynonymCollection Synonyms { get; protected set; }

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

        public static implicit operator string(LexicalItem obj)
        {
            return ReferenceEquals(null, obj)
                       ? null
                       : obj.ToString();
        }

        public LexicalMatch Match(string value)
        {
            var x = _canonicalForm.Key;
            var y = Comparer.Normalize(value);

            if (Comparer.Equals(x, y))
            {
                return new LexicalMatch(this);
            }

            if (Synonyms.Contains(y))
            {
                return new LexicalMatch(this);
            }

            return null;
        }

        public LexicalMatch MatchBeginning(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Length)
            {
                return null;
            }

            var result = Match(value);
            if (null != result)
            {
                return result;
            }

            var substring = new MutableString(value);
            var clone = substring.Clone();
#if NET20
            foreach (var word in IEnumerableExtensionMethods.Reverse(substring.ToString().Split(' ')))
#else
            foreach (var word in substring.ToString().Split(' ').Reverse())
#endif
            {
                if (0 == word.Length)
                {
                    continue;
                }

                substring.RemoveFromEnd(word).Trim();
                result = Match(substring);
                if (null == result)
                {
                    continue;
                }

                result.Suffix = clone.Suffix(substring).Trim();
                return result;
            }

            return null;
        }

        public LexicalMatch MatchEnding(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Length)
            {
                return null;
            }

            var result = Match(value);
            if (null != result)
            {
                return result;
            }

            var substring = new MutableString(value);
            var clone = substring.Clone();
#if NET20
            foreach (var word in IEnumerableExtensionMethods.ToList(substring.ToString().Split(' ')))
#else
            foreach (var word in substring.ToString().Split(' ').ToList())
#endif
            {
                substring.RemoveFromStart(word).Trim();
                result = Match(substring);
                if (null == result)
                {
                    continue;
                }

                result.Prefix = clone.Prefix(substring).Trim();
                return result;
            }

            return null;
        }

        public LexicalMatch MatchWithin(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (0 == value.Length)
            {
                return null;
            }

            var result = Match(value);
            if (null != result)
            {
                return result;
            }

            var parts = value.Split(' ');
            if (parts.Length < 2)
            {
                return null;
            }

            var mutable = new MutableString(value);
#if NET20
            foreach (var part in PartsWithin(IEnumerableExtensionMethods.ToList(mutable.ToString().Split(' '))))
#else
            foreach (var part in PartsWithin(mutable.ToString().Split(' ').ToList()))
#endif
            {
                result = Match(part);
                if (null == result)
                {
                    continue;
                }

#if NET20
                var splits = StringExtensionMethods.Split(value, part, StringSplitOptions.None);
#else
                var splits = value.Split(part, StringSplitOptions.None);
#endif
                if (2 != splits.Length)
                {
                    continue;
                }

                result.Prefix = splits[0].Trim();
                result.Suffix = splits[1].Trim();
                return result;
            }

            return null;
        }

        public virtual bool Contains(string value)
        {
            return null != Match(value);
        }

#if !NET20
        public virtual void Invoke(Func<string, string> func)
        {
            if (null == func)
            {
                throw new ArgumentNullException("func");
            }

            CanonicalForm = func.Invoke(CanonicalForm);
            var synonyms = Synonyms.ToList();
            Synonyms.Clear();
            foreach (var synonym in synonyms.OrderBy(x => x))
            {
                Synonyms.Add(func.Invoke(synonym));
            }
        }

#endif

        public override string ToString()
        {
            return CanonicalForm ?? string.Empty;
        }

        private static IEnumerable<string> PartsWithin(IList<string> parts)
        {
            if (null == parts)
            {
                yield break;
            }

            if (0 == parts.Count)
            {
                yield break;
            }

            foreach (var part in parts)
            {
                yield return part;
            }

            for (var i = 2; i < parts.Count; i++)
            {
                for (var j = 0; j < parts.Count; j++)
                {
                    if (i + j > parts.Count)
                    {
                        break;
                    }

                    var result = new MutableString();
                    for (var x = j; x < j + i; x++)
                    {
                        result.Append(' ');
                        result.Append(parts[x]);
                    }

                    yield return result.Trim();
                }
            }
        }
    }
}