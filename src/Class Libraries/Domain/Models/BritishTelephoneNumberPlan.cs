namespace Cavity.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using Cavity.Collections;
    using Cavity.Data;

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The naming is intentional.")]
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "This class doesn't need binary serialization.")]
    public class BritishTelephoneNumberPlan : Dictionary<string, BritishTelephoneNumberPlanItem>
    {
        public BritishTelephoneNumberPlan()
            : base(StringComparer.Ordinal)
        {
        }

        public static BritishTelephoneNumberPlan Load(FileInfo file)
        {
            var result = new BritishTelephoneNumberPlan();

            foreach (var item in new CsvDataSheet(file).Where(entry => entry["Status"].Is("Designated"))
                                                       .Select(Load))
            {
                result.TryAdd(item.DialingCode, item);
            }

            return result;
        }

        public static BritishTelephoneNumberPlanItem Load(KeyStringDictionary entry)
        {
            if (null == entry)
            {
                throw new ArgumentNullException("entry");
            }

            var dialing = "{0}{1}".FormatWith(entry["Code"], entry["D/DE"]);

            string area;
            if (dialing.StartsWith("2", StringComparison.Ordinal))
            {
                area = dialing.Substring(0, 2);
            }
            else if (dialing.Substring(0, 3).In("113", "114", "115", "116", "117", "118", "121", "131", "141", "151", "161", "191"))
            {
                area = dialing.Substring(0, 3);
            }
            else
            {
                switch (entry["Notes"])
                {
                    case "3 Digit Area Code":
                    case "3 Digit Code Area":
                        area = dialing.Substring(0, 3);
                        break;
                    case "4 Digit Code Area":
                        area = dialing.Substring(0, 4);
                        break;
                    case "5 Digit Code Area":
                        area = dialing.Substring(0, 5);
                        break;
                    default:
                        area = dialing;
                        break;
                }
            }

            return new BritishTelephoneNumberPlanItem
                       {
                           AreaCode = area,
                           DialingCode = dialing,
                           Use = entry["Use"],
                       };
        }

        public BritishTelephoneNumberPlanItem Item(string telephone)
        {
            return Item(Telephone.FromString(telephone));
        }

        private BritishTelephoneNumberPlanItem Item(Telephone telephone)
        {
            if (null == telephone)
            {
                throw new ArgumentNullException("telephone");
            }

            if (null == telephone.Number)
            {
                return null;
            }

            if (telephone.Number.StartsWithAny(StringComparison.Ordinal, "+441", "+442"))
            {
                if (BritishTelephoneNumberLength.Validate(telephone))
                {
                    return new[] { 5, 4, 3 }.Select(length => telephone.Number.Substring(3, length))
                                            .Where(ContainsKey).Select(prefix => this[prefix])
                                            .FirstOrDefault();
                }
            }

            if (telephone.Number.Length.IsNot(13))
            {
                return null;
            }

            if (telephone.Number.StartsWith("+447", StringComparison.Ordinal))
            {
                var code = telephone.Number.Substring(3, 4);
                return new BritishTelephoneNumberPlanItem
                           {
                               AreaCode = code,
                               DialingCode = code,
                               Use = string.Empty
                           };
            }

            return null;
        }
    }
}