namespace Cavity.Models
{
    using System;
    using System.Collections.Generic;
    using Cavity.Collections;

    public static class BritishTelephoneNumberLength
    {
        private static readonly HashSet<string> _twelves = Twelves().ToHashSet(StringComparer.OrdinalIgnoreCase);

        public static bool Validate(Telephone telephone)
        {
            if (null == telephone)
            {
                throw new ArgumentNullException("telephone");
            }

            return Validate(telephone.Number);
        }

        private static IEnumerable<string> Twelves()
        {
            yield return "+441204";
            yield return "+441208";
            yield return "+441254";
            yield return "+441276";
            yield return "+441297";
            yield return "+441298";
            yield return "+441363";
            yield return "+441364";
            yield return "+441384";
            yield return "+441386";
            yield return "+441404";
            yield return "+441420";
            yield return "+441460";
            yield return "+441461";
            yield return "+441480";
            yield return "+441488";
            yield return "+441524";
            yield return "+441527";
            yield return "+441562";
            yield return "+441566";
            yield return "+441606";
            yield return "+441629";
            yield return "+441635";
            yield return "+441647";
            yield return "+441659";
            yield return "+441695";
            yield return "+441697";
            yield return "+441726";
            yield return "+441744";
            yield return "+441750";
            yield return "+441768";
            yield return "+441827";
            yield return "+441837";
            yield return "+441884";
            yield return "+441900";
            yield return "+441905";
            yield return "+441935";
            yield return "+441946";
            yield return "+441949";
            yield return "+441963";
            yield return "+441995";
        }

        private static bool Validate(string telephone)
        {
            if (null == telephone)
            {
                throw new ArgumentNullException("telephone");
            }

            if (telephone.Is("+448001111"))
            {
                return true;
            }

            if (telephone.Length.IsLessThan(12))
            {
                return false;
            }

            if (telephone.StartsWith("+44", StringComparison.Ordinal).IsFalse())
            {
                return false;
            }

            if (telephone.StartsWith("+44500", StringComparison.Ordinal) ||
                telephone.StartsWith("+44800", StringComparison.Ordinal))
            {
                return telephone.Length.In(12, 13);
            }

            var prefix = telephone.Substring(0, 7);

            return _twelves.Contains(prefix)
                       ? telephone.Length.In(12, 13)
                       : telephone.Length.Is(13);
        }
    }
}