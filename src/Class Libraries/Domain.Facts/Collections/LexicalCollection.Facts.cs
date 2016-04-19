namespace Cavity.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cavity.Models;
    using Xunit;

    public sealed class LexicalCollectionFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<LexicalCollection>()
                            .DerivesFrom<object>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .NoDefaultConstructor()
                            .Implements<IEnumerable<LexicalItem>>()
                            .Result);
        }

        [Fact]
        public void ctor_INormalityComparer()
        {
            Assert.NotNull(new LexicalCollection(NormalityComparer.Ordinal));
        }

        [Fact]
        public void ctor_INormalityComparerNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LexicalCollection(null));
        }

        [Fact]
        public void indexer_stringNull_get()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);

            Assert.Throws<ArgumentNullException>(() => obj[null]);
        }

        [Fact]
        public void indexer_string_get()
        {
            const string expected = "Example";

            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              expected
                          };

            var actual = obj[expected].CanonicalForm;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void indexer_string_getWhenNotFound()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);

            Assert.Null(obj["Example"]);
        }

        [Fact]
        public void indexer_string_getWhenSynonym()
        {
            const string expected = "Foo";
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add(expected).Synonyms.Add("Bar");

            var actual = obj["Bar"].CanonicalForm;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void op_Add_LexicalItem()
        {
            const string expected = "Example";

            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              new LexicalItem(NormalityComparer.CurrentCulture, expected)
                          };

            var actual = obj.First().CanonicalForm;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Add_LexicalItemNull()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);

            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => obj.Add(null as LexicalItem));

            // ReSharper restore RedundantCast
        }

        [Fact]
        public void op_Add_string()
        {
            const string expected = "Example";

            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              expected
                          };

            var actual = obj.First().CanonicalForm;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Add_stringEmpty()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LexicalCollection(NormalityComparer.Ordinal).Add(string.Empty));
        }

        [Fact]
        public void op_Add_stringNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LexicalCollection(NormalityComparer.Ordinal).Add(null as string));
        }

        [Fact]
        public void op_Add_string_alreadyExists()
        {
            const string expected = "Example";

            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              expected,
                              expected
                          };

            Assert.Equal(1, obj.Count());
        }

        [Fact]
        public void op_Add_string_alreadyExistsAsSynonym()
        {
            const string expected = "foo";

            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add("bar").Synonyms.Add(expected);

            obj.Add(expected);

            Assert.Equal(1, obj.Count());
        }

        [Fact]
        public void op_Contains_string()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              "Example"
                          };

            Assert.True(obj.Contains("Example"));
        }

        [Fact]
        public void op_Contains_stringEmpty()
        {
            Assert.False(new LexicalCollection(NormalityComparer.Ordinal).Contains(string.Empty));
        }

        [Fact]
        public void op_Contains_stringEmpty_whenOrdinalComparer()
        {
            Assert.False(new LexicalCollection(NormalityComparer.Ordinal).Contains(string.Empty));
        }

        [Fact]
        public void op_Contains_stringNull()
        {
            Assert.False(new LexicalCollection(NormalityComparer.Ordinal).Contains(null));
        }

        [Fact]
        public void op_Contains_stringNull_whenOrdinalComparer()
        {
            Assert.False(new LexicalCollection(NormalityComparer.Ordinal).Contains(null));
        }

        [Fact]
        public void op_Contains_string_whenEmpty()
        {
            Assert.False(new LexicalCollection(NormalityComparer.Ordinal).Contains("Example"));
        }

        [Fact]
        public void op_Contains_string_whenEmptyWithOrdinalComparer()
        {
            Assert.False(new LexicalCollection(NormalityComparer.Ordinal).Contains("Example"));
        }

        [Fact]
        public void op_Contains_string_whenOrdinalIgnoreCaseComparer()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              "Example"
                          };

            Assert.True(obj.Contains("EXAMPLE"));
        }

        [Fact]
        public void op_Invoke_Func()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add(string.Concat("Foo", '\u00A0', "Bar")).Synonyms.Add(string.Concat("Left", '\u00A0', "Right"));

            obj.Invoke(x => x.NormalizeWhiteSpace());

            Assert.Equal("Foo Bar", obj.First().CanonicalForm);
            Assert.Equal("Left Right", obj.First().Synonyms.First());
        }

        [Fact]
        public void op_Invoke_FuncNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LexicalCollection(NormalityComparer.Ordinal).Invoke(null));
        }

        [Fact]
        public void op_MatchBeginning_string()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            var expected = new LexicalMatch(obj.First())
                               {
                                   Suffix = "test case"
                               };
            var actual = obj.MatchBeginning("example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchBeginning_stringEmpty()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Null(obj.MatchBeginning(string.Empty));
        }

        [Fact]
        public void op_MatchBeginning_stringNull()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Throws<ArgumentNullException>(() => obj.MatchBeginning(null));
        }

        [Fact]
        public void op_MatchBeginning_string_whenContainsDoubleSpace()
        {
            const string expected = "Example";

            var obj = new LexicalCollection(NormalityComparer.Ordinal)
                          {
                              expected
                          };

            Assert.Null(obj.MatchBeginning("a  z"));
        }

        [Fact]
        public void op_MatchBeginning_string_whenLongerAndShorter()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "one two"),
                              new LexicalItem(NormalityComparer.Ordinal, "one")
                          };

            var expected = new LexicalMatch(obj.First())
                               {
                                   Suffix = "3"
                               };
            var actual = obj.MatchBeginning("one two 3");

            Assert.Equal(expected, actual);
            Assert.Equal("3", actual.Suffix);
        }

        [Fact]
        public void op_MatchBeginning_string_whenShorterAndLonger()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "one"),
                              new LexicalItem(NormalityComparer.Ordinal, "one two")
                          };

            var expected = new LexicalMatch(obj.Last())
                               {
                                   Suffix = "3"
                               };
            var actual = obj.MatchBeginning("one two 3");

            Assert.Equal(expected, actual);
            Assert.Equal("3", actual.Suffix);
        }

        [Fact]
        public void op_MatchEnding_string()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            var expected = new LexicalMatch(obj.First())
                               {
                                   Prefix = "This is an"
                               };
            var actual = obj.MatchEnding("This is an example");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchEnding_stringEmpty()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Null(obj.MatchEnding(string.Empty));
        }

        [Fact]
        public void op_MatchEnding_stringNull()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Throws<ArgumentNullException>(() => obj.MatchEnding(null));
        }

        [Fact]
        public void op_MatchEnding_string_whenLongerAndShorter()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "two three"),
                              new LexicalItem(NormalityComparer.Ordinal, "two")
                          };

            var expected = new LexicalMatch(obj.First())
                               {
                                   Prefix = "1"
                               };
            var actual = obj.MatchEnding("1 two three");

            Assert.Equal(expected, actual);
            Assert.Equal("1", actual.Prefix);
        }

        [Fact]
        public void op_MatchEnding_string_whenShorterAndLonger()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "two"),
                              new LexicalItem(NormalityComparer.Ordinal, "two three")
                          };

            var expected = new LexicalMatch(obj.Last())
                               {
                                   Prefix = "1"
                               };
            var actual = obj.MatchEnding("1 two three");

            Assert.Equal(expected, actual);
            Assert.Equal("1", actual.Prefix);
        }

        [Fact]
        public void op_MatchWithin_string()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            var expected = new LexicalMatch(obj.First())
                               {
                                   Prefix = "This is an",
                                   Suffix = "test case"
                               };
            var actual = obj.MatchWithin("This is an example test case");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MatchWithin_stringEmpty()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Null(obj.MatchWithin(string.Empty));
        }

        [Fact]
        public void op_MatchWithin_stringNull()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Throws<ArgumentNullException>(() => obj.MatchWithin(null));
        }

        [Fact]
        public void op_MatchWithin_string_whenLongerAndShorter()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "two three"),
                              new LexicalItem(NormalityComparer.Ordinal, "three")
                          };

            var expected = new LexicalMatch(obj.First())
                               {
                                   Prefix = "1",
                                   Suffix = "4"
                               };
            var actual = obj.MatchWithin("1 two three 4");

            Assert.Equal(expected, actual);
            Assert.Equal("1", actual.Prefix);
            Assert.Equal("4", actual.Suffix);
        }

        [Fact]
        public void op_MatchWithin_string_whenShorterAndLonger()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "three"),
                              new LexicalItem(NormalityComparer.Ordinal, "two three")
                          };

            var expected = new LexicalMatch(obj.Last())
                               {
                                   Prefix = "1",
                                   Suffix = "4"
                               };
            var actual = obj.MatchWithin("1 two three 4");

            Assert.Equal(expected, actual);
            Assert.Equal("1", actual.Prefix);
            Assert.Equal("4", actual.Suffix);
        }

        [Fact]
        public void op_Match_string()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            var expected = new LexicalMatch(obj.First());
            var actual = obj.Match("EXAMPLE");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_Match_stringEmpty()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Null(obj.Match(string.Empty));
        }

        [Fact]
        public void op_Match_stringNull()
        {
            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase)
                          {
                              new LexicalItem(NormalityComparer.Ordinal, "Example")
                          };

            Assert.Throws<ArgumentNullException>(() => obj.Match(null));
        }

        [Fact]
        public void op_MoveTo_LexiconNull_LexicalItem()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            var item = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<ArgumentNullException>(() => obj.MoveTo(null, item));
        }

        [Fact]
        public void op_MoveTo_LexiconSame_LexicalItem()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            var item = new LexicalItem(NormalityComparer.Ordinal, "Example");

            Assert.Throws<InvalidOperationException>(() => obj.MoveTo(obj, item));
        }

        [Fact]
        public void op_MoveTo_Lexicon_LexicalItem()
        {
            const string expected = "Example";

            var source = new LexicalCollection(NormalityComparer.Ordinal);
            var item = source.Add(expected);

            var destination = new LexicalCollection(NormalityComparer.Ordinal);
            source.MoveTo(destination, item);

            Assert.Empty(source);
            var actual = destination.First().CanonicalForm;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_MoveTo_Lexicon_LexicalItemNull()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            var destination = new LexicalCollection(NormalityComparer.Ordinal);

            Assert.Throws<ArgumentNullException>(() => obj.MoveTo(destination, null));
        }

        [Fact]
        public void op_Remove_IEnumerableLexicalItems()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add("1").Synonyms.Add("One");

            var lexicon = new LexicalCollection(NormalityComparer.Ordinal);
            lexicon.Add("1").Synonyms.Add("One");

            obj.Remove(lexicon);

            Assert.Equal(0, obj.Count());
        }

        [Fact]
        public void op_Remove_IEnumerableLexicalItemsEmpty()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add("1").Synonyms.Add("One");

            obj.Remove(new LexicalCollection(NormalityComparer.Ordinal));

            Assert.Equal(1, obj.Count());
        }

        [Fact]
        public void op_Remove_IEnumerableLexicalItemsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Lexicon(NormalityComparer.Ordinal).Remove(null as IEnumerable<LexicalItem>));
        }

        [Fact]
        public void op_Remove_IEnumerableLexicalItemsSynonym()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add("Foo").Synonyms.Add("One");

            var lexicon = new LexicalCollection(NormalityComparer.Ordinal);
            lexicon.Add("Bar").Synonyms.Add("One");

            obj.Remove(lexicon);

            Assert.Equal(0, obj.Count());
        }

        [Fact]
        public void op_Remove_LexicalItem()
        {
            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add("1").Synonyms.Add("One");

            obj.Remove(new LexicalItem(NormalityComparer.Ordinal, "1"));

            Assert.Equal(0, obj.Count());
        }

        [Fact]
        public void op_Remove_LexicalItemsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Lexicon(NormalityComparer.Ordinal).Remove(null as LexicalItem));
        }

        [Fact]
        public void op_ToCanonicalForm_string()
        {
            const string expected = "1";

            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add(expected).Synonyms.Add("One");

            var actual = obj.ToCanonicalForm(obj.First().Synonyms.First());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_ToCanonicalForm_stringEmpty()
        {
            Assert.Null(new LexicalCollection(NormalityComparer.Ordinal).ToCanonicalForm(string.Empty));
        }

        [Fact]
        public void op_ToCanonicalForm_stringEmpty_whenInvariantComparer()
        {
            Assert.Null(new LexicalCollection(NormalityComparer.Ordinal).ToCanonicalForm(string.Empty));
        }

        [Fact]
        public void op_ToCanonicalForm_stringNull()
        {
            Assert.Null(new LexicalCollection(NormalityComparer.Ordinal).ToCanonicalForm(null));
        }

        [Fact]
        public void op_ToCanonicalForm_stringNull_whenInvariantComparer()
        {
            Assert.Null(new LexicalCollection(NormalityComparer.Ordinal).ToCanonicalForm(null));
        }

        [Fact]
        public void op_ToCanonicalForm_string_whenOrdinalIgnoreCaseComparer()
        {
            const string expected = "1";

            var obj = new LexicalCollection(NormalityComparer.OrdinalIgnoreCase);
            obj.Add(expected).Synonyms.Add("One");

            var actual = obj.ToCanonicalForm("ONE");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_CanonicalForms_get()
        {
            const string expected = "1";

            var obj = new LexicalCollection(NormalityComparer.Ordinal);
            obj.Add(expected).Synonyms.Add("One");

            var actual = obj.CanonicalForms.First();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void prop_Comparer()
        {
            Assert.True(new PropertyExpectations<LexicalCollection>(x => x.Comparer)
                            .TypeIs<INormalityComparer>()
                            .ArgumentNullException()
                            .Set(NormalityComparer.Ordinal)
                            .IsNotDecorated()
                            .Result);
        }
    }
}