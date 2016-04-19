namespace Cavity.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using Cavity.Collections;
    using Cavity.IO;
    using Cavity.Models;
    using Xunit;

    public sealed class CsvLexiconStorageFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<CsvLexiconStorage>()
                            .DerivesFrom<object>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .NoDefaultConstructor()
                            .IsNotDecorated()
                            .Implements<IStoreLexicon>()
                            .Result);
        }

        [Fact]
        public void ctor_FileInfo()
        {
            using (var file = new TempFile())
            {
                Assert.NotNull(new CsvLexiconStorage(file.Info));
            }
        }

        [Fact]
        public void ctor_FileInfoNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CsvLexiconStorage(null));
        }

        [Fact]
        public void op_Delete_Lexicon()
        {
            using (var file = new TempFile())
            {
                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                store.Delete(new Lexicon(NormalityComparer.Ordinal));

                file.Info.Refresh();
                Assert.False(file.Info.Exists);
            }
        }

        [Fact]
        public void op_Delete_LexiconNull()
        {
            using (var file = new TempFile())
            {
                IStoreLexicon store = new CsvLexiconStorage(file.Info);

                Assert.Throws<ArgumentNullException>(() => store.Delete(null));
            }
        }

        [Fact]
        public void op_LoadHierarchy_INormalityComparer()
        {
            using (var parent = new TempDirectory())
            {
                var file = parent.Info.ToFile("example.csv");

                file.AppendLine("CANONICAL,SYNONYMS");
                file.AppendLine("parent,");

                var child = parent.Info.ToDirectory("child", true);
                file = child.ToFile("example.csv");

                file.AppendLine("CANONICAL,SYNONYMS");
                file.AppendLine("child,");

                var store = new CsvLexiconStorage(file);
                var obj = store.LoadHierarchy(NormalityComparer.Ordinal);

                Assert.Equal("parent", obj.ToCanonicalForm("parent"));
                Assert.Equal("child", obj.ToCanonicalForm("child"));
            }
        }

        [Fact]
        public void op_Load_INormalityComparer()
        {
            using (var file = new TempFile())
            {
                file.Info.AppendLine("CANONICAL,SYNONYMS");
                file.Info.AppendLine("1,");

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                var obj = store.Load(NormalityComparer.Ordinal);

                Assert.Equal("1", obj.ToCanonicalForm("1"));

                Assert.Same(store, obj.Storage);
            }
        }

        [Fact]
        public void op_Load_INormalityComparerNull()
        {
            using (var file = new TempFile())
            {
                file.Info.Delete();

                var lexicon = new Lexicon(NormalityComparer.Ordinal)
                                  {
                                      "Example"
                                  };

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                store.Save(lexicon);

                Assert.Throws<ArgumentNullException>(() => store.Load(null));
            }
        }

        [Fact]
        public void op_Load_INormalityComparer_whenFileMissing()
        {
            using (var directory = new TempDirectory())
            {
                IStoreLexicon store = new CsvLexiconStorage(directory.Info.ToFile("example.csv"));

                Assert.Throws<FileNotFoundException>(() => store.Load(NormalityComparer.Ordinal));
            }
        }

        [Fact]
        public void op_Load_INormalityComparer_withMultipleSynonyms()
        {
            using (var file = new TempFile())
            {
                file.Info.AppendLine("CANONICAL,SYNONYMS");
                file.Info.AppendLine("1,One;Unit");

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                var obj = store.Load(NormalityComparer.Ordinal);

                Assert.Equal("1", obj.ToCanonicalForm("One"));
                Assert.Equal("1", obj.ToCanonicalForm("Unit"));
            }
        }

        [Fact]
        public void op_Load_withRepeats()
        {
            using (var file = new TempFile())
            {
                file.Info.AppendLine("CANONICAL,SYNONYMS");
                file.Info.AppendLine("1,One");
                file.Info.AppendLine("1,Unit");

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                var obj = store.Load(NormalityComparer.Ordinal);

                Assert.Equal("1", obj.ToCanonicalForm("One"));
                Assert.Equal("1", obj.ToCanonicalForm("Unit"));
                Assert.Equal(1, obj.Count());
                Assert.Equal(2, obj.First().Synonyms.Count);
            }
        }

        [Fact]
        public void op_Load_withSingleSynonym()
        {
            using (var file = new TempFile())
            {
                file.Info.AppendLine("CANONICAL,SYNONYMS");
                file.Info.AppendLine("1,One");

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                var obj = store.Load(NormalityComparer.Ordinal);

                Assert.Equal("1", obj.ToCanonicalForm("One"));
            }
        }

        [Fact]
        public void op_Save_Lexicon()
        {
            using (var file = new TempFile())
            {
                file.Info.Delete();

                var lexicon = new Lexicon(NormalityComparer.Ordinal)
                                  {
                                      "Example"
                                  };

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                store.Save(lexicon);

                file.Info.Refresh();
                Assert.True(file.Info.Exists);

                Assert.True(store.Load(NormalityComparer.Ordinal).Contains("Example"));
            }
        }

        [Fact]
        public void op_Save_LexiconNull()
        {
            using (var file = new TempFile())
            {
                IStoreLexicon store = new CsvLexiconStorage(file.Info);

                Assert.Throws<ArgumentNullException>(() => store.Save(null));
            }
        }

        [Fact]
        public void op_Save_LexiconWhenEmpty()
        {
            using (var file = new TempFile())
            {
                file.Info.Delete();

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                store.Save(new Lexicon(NormalityComparer.Ordinal));

                file.Info.Refresh();
                Assert.True(file.Info.Exists);

                Assert.True(File.ReadAllText(file.Info.FullName).StartsWith("CANONICAL,SYNONYMS", StringComparison.Ordinal));
            }
        }

        [Fact]
        public void op_Save_LexiconWithComma()
        {
            using (var file = new TempFile())
            {
                file.Info.Delete();

                var lexicon = new Lexicon(NormalityComparer.Ordinal)
                                  {
                                      "foo, bar"
                                  };

                IStoreLexicon store = new CsvLexiconStorage(file.Info);
                store.Save(lexicon);

                file.Info.Refresh();
                Assert.True(file.Info.Exists);

                Assert.True(store.Load(NormalityComparer.Ordinal).Contains("foo, bar"));
            }
        }

        [Fact]
        public void prop_Location()
        {
            Assert.True(new PropertyExpectations<CsvLexiconStorage>(p => p.Location)
                            .IsNotDecorated()
                            .TypeIs<FileInfo>()
                            .ArgumentNullException()
                            .Result);
        }
    }
}