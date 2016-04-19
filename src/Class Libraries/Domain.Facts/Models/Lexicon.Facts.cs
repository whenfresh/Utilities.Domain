namespace Cavity.Models
{
    using System;
    using Cavity.Collections;
    using Cavity.Data;
    using Moq;
    using Xunit;

    public sealed class LexiconFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<Lexicon>()
                            .DerivesFrom<LexicalCollection>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .NoDefaultConstructor()
                            .Result);
        }

        [Fact]
        public void ctor_IComparerNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Lexicon(null));
        }

        [Fact]
        public void ctor_INormalizationComparer()
        {
            Assert.NotNull(new Lexicon(NormalityComparer.Ordinal));
        }

        [Fact]
        public void op_Delete()
        {
            Assert.Throws<ArgumentNullException>(() => new Lexicon(NormalityComparer.Ordinal).Delete());
        }

        [Fact]
        public void op_Delete_IStoreLexicon()
        {
            var obj = new Lexicon(NormalityComparer.Ordinal);

            var storage = new Mock<IStoreLexicon>();
            storage.Setup(x => x.Delete(obj)).Verifiable();

            obj.Delete(storage.Object);

            Assert.Same(storage.Object, obj.Storage);

            storage.VerifyAll();
        }

        [Fact]
        public void op_Delete_IStoreLexiconNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Lexicon(NormalityComparer.Ordinal).Delete(null));
        }

        [Fact]
        public void op_Delete_whenStorageDefined()
        {
            var obj = new Lexicon(NormalityComparer.Ordinal);

            var storage = new Mock<IStoreLexicon>();
            storage.Setup(x => x.Delete(obj)).Verifiable();

            obj.Storage = storage.Object;

            obj.Delete();

            storage.VerifyAll();
        }

        [Fact]
        public void op_Save()
        {
            Assert.Throws<ArgumentNullException>(() => new Lexicon(NormalityComparer.Ordinal).Save());
        }

        [Fact]
        public void op_Save_IStoreLexicon()
        {
            var obj = new Lexicon(NormalityComparer.Ordinal);

            var storage = new Mock<IStoreLexicon>();
            storage.Setup(x => x.Save(obj)).Verifiable();

            obj.Save(storage.Object);

            Assert.Same(storage.Object, obj.Storage);

            storage.VerifyAll();
        }

        [Fact]
        public void op_Save_IStoreLexiconNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Lexicon(NormalityComparer.Ordinal).Save(null));
        }

        [Fact]
        public void op_Save_whenStorageDefined()
        {
            var obj = new Lexicon(NormalityComparer.Ordinal);

            var storage = new Mock<IStoreLexicon>();
            storage.Setup(x => x.Save(obj)).Verifiable();

            obj.Storage = storage.Object;

            obj.Save();

            storage.VerifyAll();
        }

        [Fact]
        public void prop_Storage()
        {
            Assert.True(new PropertyExpectations<Lexicon>(p => p.Storage)
                            .TypeIs<IStoreLexicon>()
                            .IsNotDecorated()
                            .Result);
        }
    }
}