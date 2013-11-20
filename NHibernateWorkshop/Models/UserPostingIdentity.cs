namespace NHibernateWorkshop.Models
{
    public class UserPostingIdentity : PostingIdentity
    {
        public User User { get; set; }

        public override string AuthorName
        {
            get { return User.Name; }
        }
    }
}