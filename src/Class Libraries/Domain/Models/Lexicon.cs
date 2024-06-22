namespace WhenFresh.Utilities.Domain.Models
{
    using WhenFresh.Utilities.Domain.Collections;
    using WhenFresh.Utilities.Domain.Data;

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This naming is intentional.")]
    public class Lexicon : LexicalCollection
    {
        public Lexicon(INormalityComparer comparer)
            : base(comparer)
        {
        }

        public IStoreLexicon Storage { get; set; }

        public virtual void Delete()
        {
            Delete(Storage);
        }

        public virtual void Delete(IStoreLexicon storage)
        {
            if (null == storage)
            {
                throw new ArgumentNullException("storage");
            }

            Storage = storage;
            Storage.Delete(this);
        }

        public virtual void Save()
        {
            Save(Storage);
        }

        public virtual void Save(IStoreLexicon storage)
        {
            if (null == storage)
            {
                throw new ArgumentNullException("storage");
            }

            Storage = storage;
            Storage.Save(this);
        }
    }
}