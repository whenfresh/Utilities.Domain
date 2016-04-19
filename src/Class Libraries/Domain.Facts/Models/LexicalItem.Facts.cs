namespace Cavity.Models
{
    using System;
    using System.Linq;
    using Cavity.Collections;
    using Xunit;

    public sealed class LexicalItemFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<LexicalItem>()
                            .DerivesFrom<object>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .NoDefaultConstructor()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void ctor_INormalizationComparerNull_string()
        {
            Assert.Throws<ArgumentNullException>(() => new LexicalItem(null, "Example"));
        }

        [Fact]
        public void ctor_INormalizationComparer_string()
        {
            Assert.NotNull(new LexicalItem(NormalityComparer.Ordinal, "Example"));
        }

        [Fact]
        public void opImplicit_string_LexicalItem()
        {
            const string expected = "Example";
            string actual = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Contains_stringEmpty()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.False(obj.Contains(string.Empty));
        }

        [Fact]
        public void op_Contains_stringEmpty_whenSynonyms()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example")
                          {
                              Synonyms =
                                  {
                                      "Foo",
                                      "Bar"
                                  }
                          };

            Assert.False(obj.Contains(string.Empty));
        }

        [Fact]
        public void op_Contains_stringNull()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<ArgumentNullException>(() => obj.Contains(null));
        }

        [Fact]
        public void op_Contains_stringNull_whenSynonyms()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example")
                          {
                              Synonyms =
                                  {
                                      "Foo",
                                      "Bar"
                                  }
                          };

            Assert.Throws<ArgumentNullException>(() => obj.Contains(null));
        }

        [Fact]
        public void op_Contains_string_whenMatchesSynonym()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "Example")
                          {
                              Synonyms =
                                  {
                                      "Foo",
                                      "Bar"
                                  }
                          };

            Assert.True(obj.Contains("Bar"));
        }

        [Fact]
        public void op_Contains_string_whenOrdinalIgnoreCase()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "Example");

            Assert.True(obj.Contains("EXAMPLE"));
        }

        [Fact]
        public void op_Invoke_Func()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, string.Concat("Foo", '\u00A0', "Bar"))
                          {
                              Synonyms =
                                  {
                                      string.Concat("Left", '\u00A0', "Right")
                                  }
                          };

            obj.Invoke(x => x.NormalizeWhiteSpace());

            Assert.Equal("Foo Bar", obj.CanonicalForm);
            Assert.Equal("Left Right", obj.Synonyms.First());
        }

        [Fact]
        public void op_Invoke_FuncNull()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<ArgumentNullException>(() => obj.Invoke(null));
        }

        [Fact]
        public void op_MatchBeginning()
        {
            var lexicon = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                              {
                                  "Blenheim Gate"
                              };

            var actual = lexicon.MatchBeginning("Blenheim Gate 22-24");

            Assert.NotNull(actual);
        }

        [Fact]
        public void op_MatchBeginning_stringEmpty()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Null(obj.MatchBeginning(string.Empty));
        }

        [Fact]
        public void op_MatchBeginning_stringNull()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<ArgumentNullException>(() => obj.MatchBeginning(null));
        }

        [Fact]
        public void op_MatchBeginning_string_whenBadSpellingSynonym()
        {
            var obj = new LexicalItem(new UnderscoreComparer(), "example");
            obj.Synonyms.Add("an example");

            var expected = new LexicalMatch(obj)
                               {
                                   Suffix = "test case"
                               };
            var actual = obj.MatchBeginning("an ex_ample test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchBeginning_string_whenBeginsWithCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Suffix = "test case"
                               };
            var actual = obj.MatchBeginning("example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchBeginning_string_whenCaseDiffersCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Null(obj.MatchBeginning("EXAMPLE"));
        }

        [Fact]
        public void op_MatchBeginning_string_whenContainsCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Suffix = "test case"
                               };
            var actual = obj.MatchBeginning("example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchBeginning_string_whenContainsSynonym()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "ignore");
            obj.Synonyms.Add("EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Suffix = "test case"
                               };

            var actual = obj.MatchBeginning("example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchBeginning_string_whenExactCanonical()
        {
            const string expected = "Example";
            var obj = new LexicalItem(NormalityComparer.Ordinal, expected);
            var actual = obj.MatchBeginning(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchEnding_stringEmpty()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Null(obj.MatchEnding(string.Empty));
        }

        [Fact]
        public void op_MatchEnding_stringNull()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<ArgumentNullException>(() => obj.MatchEnding(null));
        }

        [Fact]
        public void op_MatchEnding_string_whenBadSpellingSynonym()
        {
            var obj = new LexicalItem(new UnderscoreComparer(), "example");
            obj.Synonyms.Add("an example");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is"
                               };
            var actual = obj.MatchEnding("This is an ex_ample");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchEnding_string_whenCaseDiffersCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Null(obj.MatchEnding("EXAMPLE"));
        }

        [Fact]
        public void op_MatchEnding_string_whenContainsCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is an"
                               };
            var actual = obj.MatchEnding("This is an example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchEnding_string_whenContainsSynonym()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "ignore");
            obj.Synonyms.Add("EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is an"
                               };

            var actual = obj.MatchEnding("This is an example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchEnding_string_whenEndsWithCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is an"
                               };
            var actual = obj.MatchEnding("This is an example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchEnding_string_whenExactCanonical()
        {
            const string expected = "Example";
            var obj = new LexicalItem(NormalityComparer.Ordinal, expected);
            var actual = obj.MatchEnding(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_stringEmpty()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Null(obj.MatchWithin(string.Empty));
        }

        [Fact]
        public void op_MatchWithin_stringNull()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<ArgumentNullException>(() => obj.MatchWithin(null));
        }

        [Fact]
        public void op_MatchWithin_string_whenBadSpellingSynonym()
        {
            var obj = new LexicalItem(new UnderscoreComparer(), "example");
            obj.Synonyms.Add("an example");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is an",
                                   Suffix = "test case"
                               };
            var actual = obj.MatchWithin("This is an ex0ample test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenBeginsWithCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Suffix = "test case"
                               };
            var actual = obj.MatchWithin("example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenBeginsWithCanonicalTwoWords()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "AN EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Suffix = "test case"
                               };
            var actual = obj.MatchWithin("an example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenCaseDiffersCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Null(obj.MatchWithin("EXAMPLE"));
        }

        [Fact]
        public void op_MatchWithin_string_whenContainsCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is an",
                                   Suffix = "test case"
                               };
            var actual = obj.MatchWithin("This is an example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenContainsCanonicalTwoWords()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "AN EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is",
                                   Suffix = "test case"
                               };
            var actual = obj.MatchWithin("This is an example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenContainsSynonym()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "ignore");
            obj.Synonyms.Add("EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is an",
                                   Suffix = "test case"
                               };

            var actual = obj.MatchWithin("This is an example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenEndsWithCanonical()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is an"
                               };
            var actual = obj.MatchWithin("This is an example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenEndsWithCanonicalTwoWords()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "AN EXAMPLE");

            var expected = new LexicalMatch(obj)
                               {
                                   Prefix = "This is"
                               };
            var actual = obj.MatchWithin("This is an example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_string_whenExactCanonical()
        {
            const string expected = "Example";
            var obj = new LexicalItem(NormalityComparer.Ordinal, expected);
            var actual = obj.MatchWithin(expected);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Match_stringEmpty()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Null(obj.Match(string.Empty));
        }

        [Fact]
        public void op_Match_stringEmpty_whenSynonyms()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example")
                          {
                              Synonyms =
                                  {
                                      "Foo",
                                      "Bar"
                                  }
                          };

            Assert.Null(obj.Match(string.Empty));
        }

        [Fact]
        public void op_Match_stringNull()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<ArgumentNullException>(() => obj.Match(null));
        }

        [Fact]
        public void op_Match_stringNull_whenSynonyms()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "Example")
                          {
                              Synonyms =
                                  {
                                      "Foo",
                                      "Bar"
                                  }
                          };

            Assert.Throws<ArgumentNullException>(() => obj.Match(null));
        }

        [Fact]
        public void op_Match_string_whenMatchesSynonym()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "Example")
                          {
                              Synonyms =
                                  {
                                      "Foo",
                                      "Bar"
                                  }
                          };

            var expected = new LexicalMatch(obj);
            var actual = obj.Match("Bar");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Match_string_whenOrdinalIgnoreCase()
        {
            var obj = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, "Example");

            var expected = new LexicalMatch(obj);
            var actual = obj.Match("EXAMPLE");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToString()
        {
            const string expected = "Example";
            var actual = new LexicalItem(NormalityComparer.OrdinalIgnoreCase, expected).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_CanonicalForm()
        {
            Assert.True(new PropertyExpectations<LexicalItem>(p => p.CanonicalForm)
                            .TypeIs<string>()
                            .ArgumentNullException()
                            .ArgumentOutOfRangeException(string.Empty)
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_Spellings()
        {
            var obj = new LexicalItem(NormalityComparer.Ordinal, "a")
                          {
                              Synonyms =
                                  {
                                      "b",
                                      "c"
                                  }
                          };

            const string expected = "abc";
            var actual = obj
                .Spellings
                .Aggregate<string, string>(null,
                                           (x,
                                            spelling) => x + spelling);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Synonyms()
        {
            Assert.True(new PropertyExpectations<LexicalItem>(p => p.Synonyms)
                            .TypeIs<SynonymCollection>()
                            .IsNotDecorated()
                            .Result);
        }
    }
}