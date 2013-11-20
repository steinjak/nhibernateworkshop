namespace NHibernateWorkshop.Models
{
    public abstract class PostingIdentity : Entity
    {
        public abstract string AuthorName { get; }
    }
}