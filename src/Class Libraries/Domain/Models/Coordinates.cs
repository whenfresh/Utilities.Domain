namespace Cavity.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot("coordinates")]
    public struct Coordinates : IEquatable<Coordinates>
    {
        private decimal _latitude;

        private decimal _longitude;

        public Coordinates(decimal latitude,
                           decimal longitude)
            : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [XmlAttribute("latitude")]
        public decimal Latitude
        {
            get
            {
                return _latitude;
            }

            set
            {
                if (90m < value)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                if (-90m > value)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _latitude = value;
            }
        }

        [XmlAttribute("longitude")]
        public decimal Longitude
        {
            get
            {
                return _longitude;
            }

            set
            {
                if (180m < value)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                if (-180m > value)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _longitude = value;
            }
        }

        public static bool operator ==(Coordinates obj,
                                       Coordinates comparand)
        {
            return obj.Equals(comparand);
        }

        public static bool operator !=(Coordinates obj,
                                       Coordinates comparand)
        {
            return !obj.Equals(comparand);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && Equals((Coordinates)obj);
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public override string ToString()
        {
#if NET20
            return StringExtensionMethods.FormatWith("{0}, {1}", ToString(Latitude, 'N', 'S'), ToString(Longitude, 'E', 'W'));
#else
            return "{0}, {1}".FormatWith(ToString(Latitude, 'N', 'S'), ToString(Longitude, 'E', 'W'));
#endif
        }

        public bool Equals(Coordinates other)
        {
            return Latitude == other.Latitude
                   && Longitude == other.Longitude;
        }

        private static string ToString(decimal value,
                                       char positive,
                                       char negative)
        {
            if (0m == value)
            {
                return "0° 0' 0\"";
            }

            var indicator = 0m < value ? positive : negative;
            value = Math.Abs(value);
            var degrees = Math.Truncate(value);
            var fraction = value - degrees;
            value = fraction * 60;
            var minutes = Math.Truncate(value);
            fraction = value - minutes;
            var seconds = fraction * 60;

#if NET20
            return StringExtensionMethods.FormatWith("{0}° {1}' {2:0.#####}\" {3}", degrees, minutes, seconds, indicator);
#else
            return "{0}° {1}' {2:0.#####}\" {3}".FormatWith(degrees, minutes, seconds, indicator);
#endif
        }
    }
}