namespace NHibernateWorkshop.Models
{
    public class AnonymousPostingIdentity : PostingIdentity
    {
        public override string AuthorName
        {
            get { return Nick; }
        }

        public virtual string Nick { get; set; }
    }
}