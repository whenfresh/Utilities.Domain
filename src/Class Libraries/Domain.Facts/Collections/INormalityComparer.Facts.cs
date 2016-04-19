namespace Cavity.Collections
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public sealed class INormalityComparerFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<INormalityComparer>()
                            .IsInterface()
                            .Implements<IEqualityComparer<string>>()
                            .Result);
        }

        [Fact]
        public void op_Normalize_string()
        {
            const string expected = "Example";

            var mock = new Mock<INormalityComparer>();
            mock
                .Setup(x => x.Normalize(expected))
                .Returns(expected)
                .Verifiable();

            var actual = mock.Object.Normalize(expected);

            Assert.Same(expected, actual);

            mock.VerifyAll();
        }

        [Fact]
        public void prop_Comparison_get()
        {
            const StringComparison expected = StringComparison.Ordinal;

            var mock = new Mock<INormalityComparer>();
            mock
                .SetupGet(x => x.Comparison)
                .Returns(expected)
                .Verifiable();

            var actual = mock.Object.Comparison;

            Assert.Equal(expected, actual);

            mock.VerifyAll();
        }
    }
}