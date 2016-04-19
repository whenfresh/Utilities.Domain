namespace Cavity.Collections
{
    using System;
    using Xunit;

    public sealed class SoundexComparerFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<SoundexComparer>()
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

            Assert.False(SoundexComparer.Instance.Equals(value, value));
        }

        [Fact]
        public void op_Equals_string_string_whenTrue()
        {
            const string value = "example";

            Assert.True(SoundexComparer.Instance.Equals(value.Soundex(), value));
        }

        [Fact]
        public void op_GetHashCode_string()
        {
            const int expected = -1563320708;
            var actual = SoundexComparer.Instance.GetHashCode("example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringEmpty()
        {
            const int expected = 757602046;
            var actual = SoundexComparer.Instance.GetHashCode(string.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringNull()
        {
            const int expected = 0;
            var actual = SoundexComparer.Instance.GetHashCode(null);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_string()
        {
            var expected = "example".Soundex();
            var actual = SoundexComparer.Instance.Normalize("example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringEmpty()
        {
            var expected = string.Empty;
            var actual = SoundexComparer.Instance.Normalize(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringNull()
        {
            Assert.Null(SoundexComparer.Instance.Normalize(null));
        }

        [Fact]
        public void prop_Comparison()
        {
            Assert.True(new PropertyExpectations<SoundexComparer>(x => x.Comparison)
                            .IsAutoProperty<StringComparison>()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_Instance_get()
        {
            Assert.IsType<SoundexComparer>(SoundexComparer.Instance);
        }
    }
}