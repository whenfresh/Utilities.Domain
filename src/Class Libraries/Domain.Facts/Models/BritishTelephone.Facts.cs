namespace Cavity.Models
{
    using System;
    using System.IO;
    using Cavity.Collections;
    using Xunit;
    using Xunit.Extensions;

    public sealed class BritishTelephoneFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<BritishTelephone>()
                            .DerivesFrom<KeyStringDictionary>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .NoDefaultConstructor()
                            .Result);
        }

        [Fact]
        public void op_Load_BritishTelephoneNumberPlanNull_Telephone()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephone.Load(null, "(01234) 567 890"));
        }

        [Theory]
        [InlineData("(01999) 123 456", false, "Geographic - unassigned", "1999", "(01999) 123 456", "1999", "123456", "LANDLINE")]
        [InlineData("(01483) 111 222", true, "Guildford", "1483", "(01483) 111 222", "1483", "111222", "LANDLINE")]
        [InlineData("+441483111222", true, "Guildford", "1483", "(01483) 111 222", "1483", "111222", "LANDLINE")]
        [InlineData("+442079111222", true, "London", "20", "(020) 7911 1222", "2079", "111222", "LANDLINE")]
        [InlineData("+447960123123", true, "", "7960", "(07960) 123 123", "7960", "123123", "MOBILE")]
        public void op_Load_BritishTelephoneNumberPlan_Telephone(string number,
                                                                 bool valid,
                                                                 string area,
                                                                 string areaCode,
                                                                 string numberGroups,
                                                                 string dialingCode,
                                                                 string localNumber,
                                                                 string serviceType)
        {
            var plan = BritishTelephoneNumberPlan.Load(new FileInfo("sabc.csv"));
            Telephone telephone = number;

            var actual = BritishTelephone.Load(plan, telephone);

            Assert.Equal(area, actual.Area);
            Assert.Equal(areaCode, actual.AreaCode);
            Assert.Equal(numberGroups, actual.NumberGroups);
            Assert.Equal(dialingCode, actual.DialingCode);
            Assert.Equal(localNumber, actual.LocalNumber);
            Assert.Equal(serviceType, actual.ServiceType);
            Assert.Equal(telephone, actual.Number);
            Assert.Equal(valid, actual.IsValid);
        }

        [Fact]
        public void op_Load_BritishTelephoneNumberPlan_TelephoneNull()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephone.Load(new BritishTelephoneNumberPlan(), null));
        }

        [Theory]
        [InlineData("+12345678901")]
        [InlineData("(09999) 123 456")]
        public void op_Load_BritishTelephoneNumberPlan_Telephone_whenInvalid(string number)
        {
            var plan = BritishTelephoneNumberPlan.Load(new FileInfo("sabc.csv"));
            Telephone telephone = number;

            var actual = BritishTelephone.Load(plan, telephone);

            Assert.Empty(actual.Area);
            Assert.Empty(actual.AreaCode);
            Assert.Empty(actual.NumberGroups);
            Assert.Empty(actual.DialingCode);
            Assert.Empty(actual.LocalNumber);
            Assert.Empty(actual.ServiceType);
            Assert.Equal(telephone, actual.Number);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void op_ToLocalNumber_stringEmpty_string()
        {
            Assert.Empty(BritishTelephone.ToLocalNumber(string.Empty, "+441483999999"));
        }

        [Fact]
        public void op_ToLocalNumber_stringNull_string()
        {
            Assert.Empty(BritishTelephone.ToLocalNumber(null, "+441483999999"));
        }

        [Fact]
        public void op_ToLocalNumber_string_stringEmpty()
        {
            Assert.Empty(BritishTelephone.ToLocalNumber("01234", string.Empty));
        }

        [Fact]
        public void op_ToLocalNumber_string_stringNull()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephone.ToLocalNumber("01234", null));
        }

        [Fact]
        public void op_ToNumberGroups_stringEmpty_string()
        {
            Assert.Empty(BritishTelephone.ToNumberGroups(string.Empty, "+441483999999"));
        }

        [Fact]
        public void op_ToNumberGroups_stringNull_string()
        {
            Assert.Empty(BritishTelephone.ToNumberGroups(null, "+441483999999"));
        }

        [Theory]
        [InlineData("(020) 7911 1222", "20", "+442079111222")]
        [InlineData("(0113) 2111 222", "113", "+441132111222")]
        [InlineData("(01483) 999 999", "1483", "+441483999999")]
        [InlineData("(015396) 11111", "15396", "+441539611111")]
        public void op_ToNumberGroups_string_string(string expected,
                                                    string areaCode,
                                                    string telephone)
        {
            var actual = BritishTelephone.ToNumberGroups(areaCode, telephone);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToNumberGroups_string_stringNull()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephone.ToNumberGroups("01234", null));
        }

        [Theory]
        [InlineData("FICTIONAL", "+44163299999")]
        [InlineData("FICTIONAL", "+44113496099")]
        [InlineData("FICTIONAL", "+44114496099")]
        [InlineData("FICTIONAL", "+44115496099")]
        [InlineData("FICTIONAL", "+44116496099")]
        [InlineData("FICTIONAL", "+44117496099")]
        [InlineData("FICTIONAL", "+44118496099")]
        [InlineData("FICTIONAL", "+44121496099")]
        [InlineData("FICTIONAL", "+44131496099")]
        [InlineData("FICTIONAL", "+44141496099")]
        [InlineData("FICTIONAL", "+44151496099")]
        [InlineData("FICTIONAL", "+44161496099")]
        [InlineData("FICTIONAL", "+44191498099")]
        [InlineData("FICTIONAL", "+44163296099")]
        [InlineData("LANDLINE", "+44148399999")]
        [InlineData("FICTIONAL", "+44207946099")]
        [InlineData("FICTIONAL", "+44289018099")]
        [InlineData("FICTIONAL", "+44292018099")]
        [InlineData("LANDLINE", "+44207199999")]
        [InlineData("FICTIONAL", "+44306999099")]
        [InlineData("NATIONWIDE", "+44399999999")]
        [InlineData("RESERVED", "+44499999999")]
        [InlineData("VOIP", "+44569999999")]
        [InlineData("FREEPHONE", "+44500999999")]
        [InlineData("CORPORATE", "+44599999999")]
        [InlineData("PERSONAL", "+4469999999")]
        [InlineData("PERSONAL", "+44709999999")]
        [InlineData("PAGER", "+44769999999")]
        [InlineData("WIFI", "+44791199999")]
        [InlineData("FICTIONAL", "+4477009009")]
        [InlineData("MOBILE", "+44796999999")]
        [InlineData("CHILDLINE", "+448001111")]
        [InlineData("SPECIAL", "+44829999999")]
        [InlineData("SPECIAL", "+44849999999")]
        [InlineData("SPECIAL", "+44879999999")]
        [InlineData("SPECIAL", "+44899999999")]
        [InlineData("FICTIONAL", "+448081570")]
        [InlineData("FREEPHONE", "+44800999999")]
        [InlineData("FICTIONAL", "+44909879099")]
        [InlineData("PREMIUM", "+44999999999")]
        public void op_ToServiceType_string(string expected,
                                            string telephone)
        {
            var actual = BritishTelephone.ToServiceType(telephone);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToServiceType_stringNull()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephone.ToServiceType(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("example")]
        [InlineData("+44099999999")]
        public void op_ToServiceType_string_whenFormatException(string telephone)
        {
            Assert.Throws<FormatException>(() => BritishTelephone.ToServiceType(telephone));
        }

        [Fact]
        public void prop_Area()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.Area)
                            .IsNotDecorated()
                            .TypeIs<string>()
                            .ArgumentNullException()
                            .Set("Example")
                            .Result);
        }

        [Fact]
        public void prop_AreaCode()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.AreaCode)
                            .IsNotDecorated()
                            .TypeIs<string>()
                            .ArgumentNullException()
                            .Set("(01234)")
                            .Result);
        }

        [Fact]
        public void prop_DialingCode()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.DialingCode)
                            .IsNotDecorated()
                            .TypeIs<string>()
                            .ArgumentNullException()
                            .Set("01234")
                            .Result);
        }

        [Fact]
        public void prop_IsInvalid()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.IsInvalid)
                            .IsNotDecorated()
                            .IsAutoProperty(true)
                            .Result);
        }

        [Fact]
        public void prop_IsValid()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.IsValid)
                            .IsNotDecorated()
                            .IsAutoProperty<bool>()
                            .Result);
        }

        [Fact]
        public void prop_LocalNumber()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.LocalNumber)
                            .IsNotDecorated()
                            .TypeIs<string>()
                            .ArgumentNullException()
                            .Set("123456")
                            .Result);
        }

        [Fact]
        public void prop_Number()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.Number)
                            .IsNotDecorated()
                            .TypeIs<Telephone>()
                            .ArgumentNullException()
                            .Set(Telephone.FromString("(01234) 567 890"))
                            .Result);
        }

        [Fact]
        public void prop_NumberGroups()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.NumberGroups)
                            .IsNotDecorated()
                            .TypeIs<string>()
                            .ArgumentNullException()
                            .Set("(01234) 567 890")
                            .Result);
        }

        [Fact]
        public void prop_ServiceType()
        {
            Assert.True(new PropertyExpectations<BritishTelephone>(p => p.ServiceType)
                            .IsNotDecorated()
                            .TypeIs<string>()
                            .ArgumentNullException()
                            .Set("LANDLINE")
                            .Result);
        }
    }
}