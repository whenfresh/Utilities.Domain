namespace Cavity.Data
{
    using Cavity.Collections;
    using Cavity.Models;
    using Moq;
    using Xunit;

    public sealed class IStoreLexiconFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<IStoreLexicon>()
                            .IsInterface()
                            .Result);
        }

        [Fact]
        public void op_Delete_Lexicon()
        {
            var lexicon = new Lexicon(NormalityComparer.Ordinal);

            var mock = new Mock<IStoreLexicon>();
            mock
                .Setup(x => x.Delete(lexicon))
                .Verifiable();

            mock.Object.Delete(lexicon);

            mock.VerifyAll();
        }

        [Fact]
        public void op_Load_INormalizationComparer()
        {
            var expected = new Lexicon(NormalityComparer.Ordinal);

            var mock = new Mock<IStoreLexicon>();
            mock
                .Setup(x => x.Load(NormalityComparer.Ordinal))
                .Returns(expected)
                .Verifiable();

            var actual = mock.Object.Load(NormalityComparer.Ordinal);

            Assert.Same(expected, actual);

            mock.VerifyAll();
        }

        [Fact]
        public void op_Save_Lexicon()
        {
            var lexicon = new Lexicon(NormalityComparer.Ordinal);

            var mock = new Mock<IStoreLexicon>();
            mock
                .Setup(x => x.Save(lexicon))
                .Verifiable();

            mock.Object.Save(lexicon);

            mock.VerifyAll();
        }
    }
}