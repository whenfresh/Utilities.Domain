namespace Cavity.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Cavity.Collections;
    using Cavity.IO;
    using Xunit;
    using Xunit.Extensions;

    public sealed class BritishTelephoneNumberPlanFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<BritishTelephoneNumberPlan>()
                            .DerivesFrom<Dictionary<string, BritishTelephoneNumberPlanItem>>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .HasDefaultConstructor()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void ctor()
        {
            var plan = new BritishTelephoneNumberPlan
                           {
                               { "example", new BritishTelephoneNumberPlanItem() }
                           };

            Assert.True(plan.ContainsKey("example"));
            Assert.False(plan.ContainsKey("Example"));
        }

        [Fact]
        public void op_Item_stringNull()
        {
            var plan = new BritishTelephoneNumberPlan();

            Assert.Throws<ArgumentNullException>(() => plan.Item(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("+12345678901")]
        [InlineData("+22345678901")]
        [InlineData("+4445678901")]
        [InlineData("+4445678901234")]
        public void op_Item_string_whenNullResult(string telephone)
        {
            var plan = new BritishTelephoneNumberPlan();

            Assert.Null(plan.Item(telephone));
        }

        [Fact]
        public void op_Load_FileInfo()
        {
            var plan = BritishTelephoneNumberPlan.Load(new FileInfo("sabc.csv"));

            Assert.NotEmpty(plan);
        }

        [Fact]
        public void op_Load_FileInfoNotFound()
        {
            using (var temp = new TempDirectory())
            {
                var file = temp.Info.ToFile("sabc.csv");

                Assert.Throws<FileNotFoundException>(() => BritishTelephoneNumberPlan.Load(file));
            }
        }

        [Fact]
        public void op_Load_FileInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephoneNumberPlan.Load(null as FileInfo));
        }

        [Fact]
        public void op_Load_KeyStringDictionaryNull()
        {
            Assert.Throws<ArgumentNullException>(() => BritishTelephoneNumberPlan.Load(null as KeyStringDictionary));
        }

        [Fact]
        public void op_Load_KeyStringDictionary_when3DigitAreaCode()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1999" },
                                { "D/DE", string.Empty },
                                { "Notes", "3 Digit Area Code" },
                                { "Use", "Example" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("199", actual.AreaCode);
            Assert.Equal("1999", actual.DialingCode);
            Assert.Equal("Example", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_when3DigitCodeArea()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1999" },
                                { "D/DE", string.Empty },
                                { "Notes", "3 Digit Code Area" },
                                { "Use", "Example" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("199", actual.AreaCode);
            Assert.Equal("1999", actual.DialingCode);
            Assert.Equal("Example", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenColchester()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1206" },
                                { "D/DE", string.Empty },
                                { "Notes", string.Empty },
                                { "Use", "Colchester" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("1206", actual.AreaCode);
            Assert.Equal("1206", actual.DialingCode);
            Assert.Equal("Colchester", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenGlasgow()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1412" },
                                { "D/DE", string.Empty },
                                { "Notes", "3 Digit Code Area" },
                                { "Use", "Glasgow" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("141", actual.AreaCode);
            Assert.Equal("1412", actual.DialingCode);
            Assert.Equal("Glasgow", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenHarrogate()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1423" },
                                { "D/DE", "6" },
                                { "Notes", "ELNS" },
                                { "Use", "Harrogate" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("14236", actual.AreaCode);
            Assert.Equal("14236", actual.DialingCode);
            Assert.Equal("Harrogate", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenLancaster()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1524" },
                                { "D/DE", "5" },
                                { "Notes", "4 Digit Code Area" },
                                { "Use", "Lancaster" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("1524", actual.AreaCode);
            Assert.Equal("15245", actual.DialingCode);
            Assert.Equal("Lancaster", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenLeeds()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1132" },
                                { "D/DE", string.Empty },
                                { "Notes", "3 Digit Area Code" },
                                { "Use", "Leeds" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("113", actual.AreaCode);
            Assert.Equal("1132", actual.DialingCode);
            Assert.Equal("Leeds", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenLondon()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "2071" },
                                { "D/DE", string.Empty },
                                { "Notes", string.Empty },
                                { "Use", "London" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("20", actual.AreaCode);
            Assert.Equal("2071", actual.DialingCode);
            Assert.Equal("London", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenNottingham()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1152" },
                                { "D/DE", "0" },
                                { "Notes", "3 Digit Area Code" },
                                { "Use", "Nottingham" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("115", actual.AreaCode);
            Assert.Equal("11520", actual.DialingCode);
            Assert.Equal("Nottingham", actual.Use);
        }

        [Fact]
        public void op_Load_KeyStringDictionary_whenSedbergh()
        {
            var entry = new KeyStringDictionary
                            {
                                { "Code", "1539" },
                                { "D/DE", "6" },
                                { "Notes", "5 Digit Code Area" },
                                { "Use", "Sedbergh" },
                            };
            var actual = BritishTelephoneNumberPlan.Load(entry);

            Assert.Equal("15396", actual.AreaCode);
            Assert.Equal("15396", actual.DialingCode);
            Assert.Equal("Sedbergh", actual.Use);
        }
    }
}