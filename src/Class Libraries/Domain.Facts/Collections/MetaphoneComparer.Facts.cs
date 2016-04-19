namespace Cavity.Collections
{
    using System;
    using Xunit;

    public sealed class MetaphoneComparerFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<MetaphoneComparer>()
                            .DerivesFrom<object>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .NoDefaultConstructor()
                            .IsNotDecorated()
                            .Implements<INormalityComparer>()
                            .Result);
        }

        [Fact]
        public void op_Equals_string_string_whenFalse()
        {
            const string value = "example";

            Assert.False(MetaphoneComparer.Instance.Equals(value, value));
        }

        [Fact]
        public void op_Equals_string_string_whenTrue()
        {
            const string value = "example";

            Assert.True(MetaphoneComparer.Instance.Equals(value.Metaphone(), value));
        }

        [Fact]
        public void op_GetHashCode_string()
        {
            const int expected = -636176500;
            var actual = MetaphoneComparer.Instance.GetHashCode("example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringEmpty()
        {
            const int expected = 757602046;
            var actual = MetaphoneComparer.Instance.GetHashCode(string.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringNull()
        {
            const int expected = 0;
            var actual = MetaphoneComparer.Instance.GetHashCode(null);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_string()
        {
            var expected = "example".Metaphone();
            var actual = MetaphoneComparer.Instance.Normalize("example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringEmpty()
        {
            var expected = string.Empty;
            var actual = MetaphoneComparer.Instance.Normalize(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringNull()
        {
            Assert.Null(MetaphoneComparer.Instance.Normalize(null));
        }

        [Fact]
        public void prop_Comparison()
        {
            Assert.True(new PropertyExpectations<MetaphoneComparer>(x => x.Comparison)
                            .IsAutoProperty<StringComparison>()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_Instance_get()
        {
            Assert.IsType<MetaphoneComparer>(MetaphoneComparer.Instance);
        }
    }
}