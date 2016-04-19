namespace Cavity.Collections
{
    using System;
    using Xunit;

    public sealed class LevenshteinComparerFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<LevenshteinComparer>()
                            .DerivesFrom<NormalityComparer>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .HasDefaultConstructor()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void ctor()
        {
            Assert.NotNull(new LevenshteinComparer());
        }

        [Fact]
        public void ctor_int()
        {
            Assert.NotNull(new LevenshteinComparer(1));
        }

        [Fact]
        public void ctor_int0()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LevenshteinComparer(0));
        }

        [Fact]
        public void op_Equals_string_string()
        {
            Assert.False(new LevenshteinComparer().Equals("abc", "xyz"));
        }

        [Fact]
        public void op_Equals_string_string_whenAboveThreshold()
        {
            Assert.False(new LevenshteinComparer().Equals("00000000", "00xxxx00"));
        }

        [Fact]
        public void op_Equals_string_string_whenBelowThreshold()
        {
            Assert.True(new LevenshteinComparer().Equals("00000000", "000xx000"));
        }

        [Fact]
        public void op_Equals_string_string_whenEqual()
        {
            Assert.True(new LevenshteinComparer().Equals("00000000", "00000000"));
        }

        [Fact]
        public void op_Equals_string_string_whenEqualIgnoreCase()
        {
            var obj = new LevenshteinComparer
                          {
                              Comparison = StringComparison.OrdinalIgnoreCase
                          };

            Assert.True(obj.Equals("abc", "ABC"));
        }

        [Fact]
        public void op_Normalize_stringEmpty()
        {
            var expected = string.Empty;
            var actual = new LevenshteinComparer().Normalize(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringNull()
        {
            Assert.Null(new LevenshteinComparer().Normalize(null));
        }

        [Fact]
        public void op_Normalize_string_whenIgnoreCase()
        {
            var obj = new LevenshteinComparer
                          {
                              Comparison = StringComparison.OrdinalIgnoreCase
                          };
            const string expected = "ABC";
            var actual = obj.Normalize("abc");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Threshold()
        {
            Assert.True(new PropertyExpectations<LevenshteinComparer>(x => x.Threshold)
                            .TypeIs<int>()
                            .DefaultValueIs(3)
                            .IsNotDecorated()
                            .Result);
        }
    }
}