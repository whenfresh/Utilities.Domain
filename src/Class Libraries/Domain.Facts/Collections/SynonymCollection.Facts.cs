namespace Cavity.Collections
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public sealed class SynonymCollectionFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<SynonymCollection>()
                            .DerivesFrom<object>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .NoDefaultConstructor()
                            .IsNotDecorated()
                            .Implements<IEnumerable<string>>()
                            .Result);
        }

        [Fact]
        public void ctor_INormalizationComparer()
        {
            Assert.NotNull(new SynonymCollection(new Mock<INormalityComparer>().Object));
        }

        [Fact]
        public void ctor_INormalizationComparerNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SynonymCollection(null));
        }

        [Fact]
        public void op_Add_string()
        {
            new SynonymCollection(NormalityComparer.OrdinalIgnoreCase)
                {
                    "Example"
                };
        }

        [Fact]
        public void op_Add_string_whenAlreadyExists()
        {
            const string synonym = "Example";
            var obj = new SynonymCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              synonym,
                              synonym
                          };

            Assert.Equal(1, obj.Count);
        }

        [Fact]
        public void op_Clear()
        {
            var obj = new SynonymCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              "Example"
                          };

            Assert.Equal(1, obj.Count);

            obj.Clear();

            Assert.Equal(0, obj.Count);
        }

        [Fact]
        public void op_Contains_string()
        {
            var obj = new SynonymCollection(NormalityComparer.Ordinal)
                          {
                              "Example"
                          };

            Assert.False(obj.Contains("EXAMPLE"));
        }

        [Fact]
        public void op_Contains_string_whenOrdinal()
        {
            const string expected = "Example";

            var obj = new SynonymCollection(NormalityComparer.Ordinal)
                          {
                              expected
                          };

            Assert.True(obj.Contains(expected));
        }

        [Fact]
        public void op_Contains_string_whenOrdinalIgnoreCase()
        {
            var obj = new SynonymCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              "Example"
                          };

            Assert.True(obj.Contains("EXAMPLE"));
        }

        [Fact]
        public void op_GetEnumerator()
        {
            const string expected = "Example";

            var obj = new SynonymCollection(NormalityComparer.Ordinal)
                          {
                              expected
                          };

            foreach (var actual in obj)
            {
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void prop_Count()
        {
            var obj = new SynonymCollection(NormalityComparer.OrdinalIgnoreCase);

            Assert.Equal(0, obj.Count);

            obj.Add("Example");

            Assert.Equal(1, obj.Count);
        }
    }
}