namespace Cavity.Collections
{
    using System;
    using Xunit;

    public sealed class NormalityComparerFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<NormalityComparer>()
                            .DerivesFrom<object>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .HasDefaultConstructor()
                            .Implements<INormalityComparer>()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void ctor()
        {
            Assert.NotNull(new NormalityComparer());
        }

        [Fact]
        public void op_GetHashCode_string()
        {
            const int expected = 1581936741;
            var actual = new NormalityComparer().GetHashCode("example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringEmpty()
        {
            const int expected = 757602046;
            var actual = new NormalityComparer().GetHashCode(string.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringNull()
        {
            const int expected = 0;
            var actual = new NormalityComparer().GetHashCode(null);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_string()
        {
            const string expected = "An Example.";
            var actual = new NormalityComparer().Normalize(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringEmpty()
        {
            var expected = string.Empty;
            var actual = new NormalityComparer().Normalize(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringNull()
        {
            Assert.Null(new NormalityComparer().Normalize(null));
        }

        [Fact]
        public void op_Normalize_string_whenIgnoreCase()
        {
            var obj = new NormalityComparer
                          {
                              Comparison = StringComparison.OrdinalIgnoreCase
                          };
            const string expected = "AN EXAMPLE.";
            var actual = obj.Normalize("An Example.");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Comparison()
        {
            Assert.True(new PropertyExpectations<NormalityComparer>(x => x.Comparison)
                            .TypeIs<StringComparison>()
                            .DefaultValueIs(StringComparison.Ordinal)
                            .Set(StringComparison.OrdinalIgnoreCase)
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_CurrentCulture()
        {
            const StringComparison expected = StringComparison.CurrentCulture;
            var actual = NormalityComparer.CurrentCulture.Comparison;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Ordinal()
        {
            const StringComparison expected = StringComparison.Ordinal;
            var actual = NormalityComparer.Ordinal.Comparison;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_OrdinalIgnoreCase()
        {
            const StringComparison expected = StringComparison.OrdinalIgnoreCase;
            var actual = NormalityComparer.OrdinalIgnoreCase.Comparison;

            Assert.Equal(expected, actual);
        }
    }
}