namespace Cavity.Collections
{
    using System;
    using Xunit;

    public sealed class CaverphoneComparerFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<CaverphoneComparer>()
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

            Assert.False(CaverphoneComparer.Instance.Equals(value, value));
        }

        [Fact]
        public void op_Equals_string_string_whenTrue()
        {
            const string value = "example";

            Assert.True(CaverphoneComparer.Instance.Equals(value.Caverphone(), value));
        }

        [Fact]
        public void op_GetHashCode_string()
        {
            const int expected = 1218658690;
            var actual = CaverphoneComparer.Instance.GetHashCode("example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringEmpty()
        {
            const int expected = 757602046;
            var actual = CaverphoneComparer.Instance.GetHashCode(string.Empty);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_GetHashCode_stringNull()
        {
            const int expected = 0;
            var actual = CaverphoneComparer.Instance.GetHashCode(null);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_string()
        {
            var expected = "example".Caverphone();
            var actual = CaverphoneComparer.Instance.Normalize("example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringEmpty()
        {
            var expected = string.Empty;
            var actual = CaverphoneComparer.Instance.Normalize(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Normalize_stringNull()
        {
            Assert.Null(CaverphoneComparer.Instance.Normalize(null));
        }

        [Fact]
        public void prop_Comparison()
        {
            Assert.True(new PropertyExpectations<CaverphoneComparer>(x => x.Comparison)
                            .IsAutoProperty<StringComparison>()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_Instance_get()
        {
            Assert.IsType<CaverphoneComparer>(CaverphoneComparer.Instance);
        }
    }
}