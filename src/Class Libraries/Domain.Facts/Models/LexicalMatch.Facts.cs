namespace Cavity.Models
{
    using System;
    using Cavity.Collections;
    using Xunit;

    public sealed class LexicalMatchFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<LexicalMatch>()
                            .DerivesFrom<ComparableObject>()
                            .IsConcreteClass()
                            .IsSealed()
                            .NoDefaultConstructor()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void ctor_LexicalItem()
        {
            Assert.NotNull(new LexicalMatch(new LexicalItem(NormalityComparer.Ordinal, "example")));
        }

        [Fact]
        public void ctor_LexicalItemNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LexicalMatch(null));
        }

        [Fact]
        public void op_ToString()
        {
            const string expected = "An example test";
            var obj = new LexicalMatch(new LexicalItem(NormalityComparer.Ordinal, "example"))
                          {
                              Prefix = "An",
                              Suffix = "test"
                          };

            var actual = obj.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Item()
        {
            Assert.True(new PropertyExpectations<LexicalMatch>(x => x.Item)
                            .TypeIs<LexicalItem>()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_Prefix()
        {
            Assert.True(new PropertyExpectations<LexicalMatch>(x => x.Prefix)
                            .IsAutoProperty<string>()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_Suffix()
        {
            Assert.True(new PropertyExpectations<LexicalMatch>(x => x.Suffix)
                            .IsAutoProperty<string>()
                            .IsNotDecorated()
                            .Result);
        }
    }
}