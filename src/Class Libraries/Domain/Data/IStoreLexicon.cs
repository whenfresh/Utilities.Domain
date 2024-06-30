namespace WhenFresh.Utilities.Data;

using WhenFresh.Utilities.Collections;
using WhenFresh.Utilities.Models;

public interface IStoreLexicon
{
    void Delete(Lexicon lexicon);

    Lexicon Load(INormalityComparer comparer);

    void Save(Lexicon lexicon);
}