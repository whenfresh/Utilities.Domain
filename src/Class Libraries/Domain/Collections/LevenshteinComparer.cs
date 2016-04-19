namespace Cavity.Collections
{
    using System;

    public class LevenshteinComparer : NormalityComparer
    {
        public LevenshteinComparer()
            : this(3)
        {
        }

        public LevenshteinComparer(int threshold)
        {
            if (1 > threshold)
            {
                throw new ArgumentOutOfRangeException("threshold");
            }

            Threshold = threshold;
        }

        public int Threshold { get; private set; }

        public override bool Equals(string x,
                                    string y)
        {
            if (null == (x ?? y))
            {
                return true;
            }

            if (null == y)
            {
                return false;
            }

            var threshold = Math.Abs(Threshold < y.Length ? (y.Length / Threshold) : 0);
            if (0 == threshold)
            {
                return base.Equals(x, y);
            }

            y = Normalize(y);
            if (string.Equals(x, y, Comparison))
            {
                return true;
            }

#if NET20
            return (threshold + 1) > StringExtensionMethods.LevenshteinDistance(x, y);
#else
            return (threshold + 1) > x.LevenshteinDistance(y);
#endif
        }
    }
}