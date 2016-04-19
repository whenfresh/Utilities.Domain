namespace Cavity.Models
{
    using System;
    using Cavity.Xml.XPath;
    using Xunit;

    public sealed class CoordinatesFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<Coordinates>()
                            .IsValueType()
                            .XmlRoot("coordinates")
                            .Implements<IEquatable<Coordinates>>()
                            .Result);
        }

        [Fact]
        public void ctor()
        {
            Assert.NotNull(new Coordinates());
        }

        [Fact]
        public void ctor_decimal_decimal()
        {
            Assert.NotNull(new Coordinates(53.493192m, -2.105470m));
        }

        [Fact]
        public void opEquality_Coordinates_Coordinates()
        {
            var obj = new Coordinates
                          {
                              Latitude = 53.493192m,
                              Longitude = -2.105470m
                          };

            var comparand = new Coordinates
                                {
                                    Latitude = 53.493192m,
                                    Longitude = -2.105470m
                                };

            Assert.True(obj == comparand);
        }

        [Fact]
        public void opInequality_Coordinates_Coordinates()
        {
            var obj = new Coordinates();
            var comparand = new Coordinates
                                {
                                    Latitude = 53.493192m,
                                    Longitude = -2.105470m
                                };

            Assert.True(obj != comparand);
        }

        [Fact]
        public void op_Equals_object()
        {
            var obj = new Coordinates
                          {
                              Latitude = 53.493192m,
                              Longitude = -2.105470m
                          };

            var comparand = new Coordinates
                                {
                                    Latitude = 53.493192m,
                                    Longitude = -2.105470m
                                };

            Assert.True(obj.Equals(comparand));
        }

        [Fact]
        public void op_Equals_objectDiffers()
        {
            var comparand = new Coordinates
                                {
                                    Latitude = 53.493192m,
                                    Longitude = -2.105470m
                                };

            Assert.False(new Coordinates().Equals(comparand));
        }

        [Fact]
        public void op_Equals_objectInvalidCast()
        {
            var obj = new Uri("http://example.com/");

            Assert.Throws<InvalidCastException>(() => new Coordinates().Equals(obj));
        }

        [Fact]
        public void op_Equals_objectNull()
        {
            Assert.False(new Coordinates().Equals(null));
        }

        [Fact]
        public void op_GetHashCode()
        {
            const int expected = -841705376;
            var obj = new Coordinates
                          {
                              Latitude = 53.493192m,
                              Longitude = -2.105470m
                          };

            var actual = obj.GetHashCode();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToString()
        {
            const string expected = "0° 0' 0\", 0° 0' 0\"";
            var actual = new Coordinates().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToString_GreenwichRoyalObservatory()
        {
            const string expected = "51° 28' 40.35\" N, 0° 0' 4.518\" W";
            var actual = new Coordinates(51.477875m, -0.001255m).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToString_WashingtonMonument()
        {
            const string expected = "38° 53' 22.40268\" N, 77° 2' 6.91656\" W";
            var actual = new Coordinates(38.8895563m, -77.0352546m).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Latitude()
        {
            Assert.True(new PropertyExpectations<Coordinates>(x => x.Latitude)
                            .TypeIs<decimal>()
                            .DefaultValueIs(0m)
                            .Set(90m)
                            .Set(-90m)
                            .ArgumentOutOfRangeException(90.1m)
                            .ArgumentOutOfRangeException(-90.1m)
                            .XmlAttribute("latitude")
                            .Result);
        }

        [Fact]
        public void prop_Longitude()
        {
            Assert.True(new PropertyExpectations<Coordinates>(x => x.Longitude)
                            .TypeIs<decimal>()
                            .DefaultValueIs(0m)
                            .Set(180m)
                            .Set(-180m)
                            .ArgumentOutOfRangeException(180.1m)
                            .ArgumentOutOfRangeException(-180.1m)
                            .XmlAttribute("longitude")
                            .Result);
        }

        [Fact]
        public void xml_deserialize()
        {
            var expected = new Coordinates
                               {
                                   Latitude = 53.493192m,
                                   Longitude = -2.105470m
                               };
            var actual = "<coordinates latitude='53.493192' longitude='-2.105470' />".XmlDeserialize<Coordinates>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void xml_deserialize_whenEmpty()
        {
            var expected = new Coordinates();
            var actual = "<coordinates />".XmlDeserialize<Coordinates>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void xml_serialize()
        {
            var obj = new Coordinates
                          {
                              Latitude = 53.493192m,
                              Longitude = -2.105470m
                          };

            var navigator = obj.XmlSerialize().CreateNavigator();

            Assert.True(navigator.Evaluate<bool>("1=count(/coordinates[@latitude='53.493192'][@longitude='-2.105470'])"));
        }
    }
}