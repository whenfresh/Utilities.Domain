namespace Cavity.Models
{
    using System;
    using Cavity.Collections;
    using Xunit;

    public sealed class LexiconStringExtensionMethodsFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(typeof(LexiconStringExtensionMethods).IsStatic());
        }

        [Fact]
        public void op_RemoveMatch_stringEmpty_Lexicon()
        {
            var expected = string.Empty;
            var actual = string.Empty.RemoveMatch(new Lexicon(NormalityComparer.Ordinal));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_RemoveMatch_stringNull_Lexicon()
        {
            Assert.Null((null as string).RemoveMatch(new Lexicon(NormalityComparer.Ordinal)));
        }

        [Fact]
        public void op_RemoveMatch_string_Lexicon()
        {
            const string expected = "Foo";

            var lexicon = new Lexicon(NormalityComparer.Ordinal)
                              {
                                  "Bar"
                              };

            var actual = expected.RemoveMatch(lexicon);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_RemoveMatch_string_LexiconNull()
        {
            Assert.Throws<ArgumentNullException>(() => "Example".RemoveMatch(null));
        }

        [Fact]
        public void op_RemoveMatch_string_Lexicon_whenMatch()
        {
            var expected = string.Empty;

            var lexicon = new Lexicon(NormalityComparer.Ordinal)
                              {
                                  "Example"
                              };

            var actual = "Example".RemoveMatch(lexicon);

            Assert.Equal(expected, actual);
        }
    }
}