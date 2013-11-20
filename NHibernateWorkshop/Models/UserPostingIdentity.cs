namespace NHibernateWorkshop.Models
{
    public class UserPostingIdentity : PostingIdentity
    {
        public virtual User User { get; set; }

        public override string AuthorName
        {
            get { return User.Name; }
        }
    }
}