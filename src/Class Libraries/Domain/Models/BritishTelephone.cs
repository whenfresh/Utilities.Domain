namespace Cavity.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Cavity.Collections;

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This naming is intentional.")]
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "This class doesn't need binary serialization.")]
    public class BritishTelephone : KeyStringDictionary
    {
        private BritishTelephone()
        {
        }

        public string Area
        {
            get
            {
                return ContainsKey("TELEPHONE AREA")
                           ? this["TELEPHONE AREA"]
                           : null;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                if (ContainsKey("TELEPHONE AREA"))
                {
                    this["TELEPHONE AREA"] = value;
                }
                else
                {
                    Add("TELEPHONE AREA", value);
                }
            }
        }

        public string AreaCode
        {
            get
            {
                return ContainsKey("TELEPHONE AREA CODE")
                           ? this["TELEPHONE AREA CODE"]
                           : null;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                if (ContainsKey("TELEPHONE AREA CODE"))
                {
                    this["TELEPHONE AREA CODE"] = value;
                }
                else
                {
                    Add("TELEPHONE AREA CODE", value);
                }
            }
        }

        public string DialingCode
        {
            get
            {
                return ContainsKey("TELEPHONE DIALING CODE")
                           ? this["TELEPHONE DIALING CODE"]
                           : null;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                if (ContainsKey("TELEPHONE DIALING CODE"))
                {
                    this["TELEPHONE DIALING CODE"] = value;
                }
                else
                {
                    Add("TELEPHONE DIALING CODE", value);
                }
            }
        }

        public bool IsInvalid
        {
            get
            {
                return !IsValid;
            }

            set
            {
                IsValid = !value;
            }
        }

        public bool IsValid { get; set; }

        public string LocalNumber
        {
            get
            {
                return ContainsKey("TELEPHONE LOCAL NUMBER")
                           ? this["TELEPHONE LOCAL NUMBER"]
                           : null;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                if (ContainsKey("TELEPHONE LOCAL NUMBER"))
                {
                    this["TELEPHONE LOCAL NUMBER"] = value;
                }
                else
                {
                    Add("TELEPHONE LOCAL NUMBER", value);
                }
            }
        }

        public Telephone Number
        {
            get
            {
                return ContainsKey("TELEPHONE")
                           ? this["TELEPHONE"]
                           : null;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                if (ContainsKey("TELEPHONE"))
                {
                    this["TELEPHONE"] = value.Number;
                }
                else
                {
                    Add("TELEPHONE", value.Number);
                }
            }
        }

        public string NumberGroups
        {
            get
            {
                return ContainsKey("TELEPHONE NUMBER GROUPS")
                           ? this["TELEPHONE NUMBER GROUPS"]
                           : null;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                if (ContainsKey("TELEPHONE NUMBER GROUPS"))
                {
                    this["TELEPHONE NUMBER GROUPS"] = value;
                }
                else
                {
                    Add("TELEPHONE NUMBER GROUPS", value);
                }
            }
        }

        public string ServiceType
        {
            get
            {
                return ContainsKey("TELEPHONE SERVICE TYPE")
                           ? this["TELEPHONE SERVICE TYPE"]
                           : null;
            }

            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("value");
                }

                if (ContainsKey("TELEPHONE SERVICE TYPE"))
                {
                    this["TELEPHONE SERVICE TYPE"] = value;
                }
                else
                {
                    Add("TELEPHONE SERVICE TYPE", value);
                }
            }
        }

        public static BritishTelephone Load(BritishTelephoneNumberPlan plan,
                                            Telephone telephone)
        {
            if (null == plan)
            {
                throw new ArgumentNullException("plan");
            }

            if (null == telephone)
            {
                throw new ArgumentNullException("telephone");
            }

            var result = new BritishTelephone
                             {
                                 Number = telephone,
                                 AreaCode = string.Empty,
                                 Area = string.Empty,
                                 NumberGroups = string.Empty,
                                 DialingCode = string.Empty,
                                 LocalNumber = string.Empty,
                                 ServiceType = string.Empty,
                             };

            var item = plan.Item(telephone);
            if (null == item)
            {
                return result;
            }

            var valid = item.Use.IndexOf("Unassigned", StringComparison.OrdinalIgnoreCase).Is(-1);

            result.Number = telephone;
            result.AreaCode = item.AreaCode;
            result.Area = item.Use;
            result.LocalNumber = ToLocalNumber(item.DialingCode, telephone.Number);
            result.NumberGroups = ToNumberGroups(item.AreaCode, telephone.Number);
            result.DialingCode = item.DialingCode;
            result.ServiceType = ToServiceType(telephone.Number);
            result.IsValid = valid;

            return result;
        }

        public static string ToLocalNumber(string dialingCode,
                                           string telephone)
        {
            if (null == telephone)
            {
                throw new ArgumentNullException("telephone");
            }

            return dialingCode.IsNullOrEmpty() || telephone.IsEmpty()
                       ? string.Empty
                       : telephone.Substring(3).RemoveFromStart(dialingCode, StringComparison.Ordinal);
        }

        public static string ToNumberGroups(string areaCode,
                                            string telephone)
        {
            if (null == telephone)
            {
                throw new ArgumentNullException("telephone");
            }

            if (null == areaCode)
            {
                return string.Empty;
            }

            if (areaCode.IsEmpty())
            {
                return string.Empty;
            }

            var number = telephone.Substring(3);

            switch (areaCode.Length)
            {
                case 2:
                    return "(0{0}) {1} {2}".FormatWith(number.Substring(0, 2), number.Substring(2, 4), number.Substring(6));
                case 3:
                    number = number.RemoveFromStart(areaCode, StringComparison.Ordinal);
                    return "(0{0}) {1} {2}".FormatWith(areaCode, number.Substring(0, 4), number.Substring(4));
                case 4:
                    number = number.RemoveFromStart(areaCode, StringComparison.Ordinal);
                    return "(0{0}) {1} {2}".FormatWith(areaCode, number.Substring(0, 3), number.Substring(3));
                default:
                    return "(0{0}) {1}".FormatWith(areaCode, number.RemoveFromStart(areaCode, StringComparison.Ordinal));
            }
        }

        public static string ToServiceType(string telephone)
        {
            if (null == telephone)
            {
                throw new ArgumentNullException("telephone");
            }

            if (telephone.Length.IsLessThan(10) ||
                telephone.StartsWith("+44", StringComparison.Ordinal).IsFalse())
            {
                throw new FormatException(telephone);
            }

            switch (telephone[3])
            {
                case '1':
                case '2':
                    return ToServiceTypeLandline(telephone);
                case '3':
                    return telephone.StartsWith("+443069990", StringComparison.Ordinal) ? "FICTIONAL" : "NATIONWIDE";
                case '4':
                    return "RESERVED";
                case '5':
                    return ToServiceTypeCorporate(telephone);
                case '6':
                    return "PERSONAL";
                case '7':
                    return ToServiceTypeMobile(telephone);
                case '8':
                    return ToServiceTypeFreephone(telephone);
                case '9':
                    return telephone.StartsWith("+449098790", StringComparison.Ordinal) ? "FICTIONAL" : "PREMIUM";
            }

            throw new FormatException(telephone);
        }

        private static string ToServiceTypeCorporate(string telephone)
        {
            if (telephone.StartsWith("+4456", StringComparison.Ordinal))
            {
                return "VOIP";
            }

            return telephone.StartsWith("+44500", StringComparison.Ordinal) ? "FREEPHONE" : "CORPORATE";
        }

        private static string ToServiceTypeFreephone(string telephone)
        {
            if (telephone.Is("+448001111"))
            {
                return "CHILDLINE";
            }

            if (telephone.StartsWith("+4482", StringComparison.Ordinal) ||
                telephone.StartsWith("+4484", StringComparison.Ordinal) ||
                telephone.StartsWith("+4487", StringComparison.Ordinal) ||
                telephone.StartsWith("+4489", StringComparison.Ordinal))
            {
                return "SPECIAL";
            }

            return telephone.StartsWith("+448081570", StringComparison.Ordinal) ? "FICTIONAL" : "FREEPHONE";
        }

        private static string ToServiceTypeLandline(string telephone)
        {
            if (telephone.StartsWith("+441632", StringComparison.Ordinal) ||
                telephone.StartsWith("+441134960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441144960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441154960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441164960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441174960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441184960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441214960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441314960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441414960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441514960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441614960", StringComparison.Ordinal) ||
                telephone.StartsWith("+441914980", StringComparison.Ordinal) ||
                telephone.StartsWith("+441632960", StringComparison.Ordinal))
            {
                return "FICTIONAL";
            }

            if (telephone.StartsWith("+442079460", StringComparison.Ordinal) ||
                telephone.StartsWith("+442890180", StringComparison.Ordinal) ||
                telephone.StartsWith("+442920180", StringComparison.Ordinal))
            {
                return "FICTIONAL";
            }

            return "LANDLINE";
        }

        private static string ToServiceTypeMobile(string telephone)
        {
            if (telephone.StartsWith("+4470", StringComparison.Ordinal))
            {
                return "PERSONAL";
            }

            if (telephone.StartsWith("+4476", StringComparison.Ordinal))
            {
                return "PAGER";
            }

            if (telephone.StartsWith("+447911", StringComparison.Ordinal))
            {
                return "WIFI";
            }

            return telephone.StartsWith("+447700900", StringComparison.Ordinal) ? "FICTIONAL" : "MOBILE";
        }
    }
}