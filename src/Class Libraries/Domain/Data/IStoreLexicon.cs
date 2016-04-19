namespace Cavity.Data
{
    using Cavity.Collections;
    using Cavity.Models;

    public interface IStoreLexicon
    {
        void Delete(Lexicon lexicon);

        Lexicon Load(INormalityComparer comparer);

        void Save(Lexicon lexicon);
    }
}