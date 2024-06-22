namespace WhenFresh.Utilities.Domain.Data
{
    using WhenFresh.Utilities.Domain.Collections;
    using WhenFresh.Utilities.Domain.Models;

    public interface IStoreLexicon
    {
        void Delete(Lexicon lexicon);

        Lexicon Load(INormalityComparer comparer);

        void Save(Lexicon lexicon);
    }
}