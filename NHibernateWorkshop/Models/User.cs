namespace NHibernateWorkshop.Models
{
    public class User : Entity
    {
        public virtual string Username { get; set; }
        public virtual string Name { get; set; }
    }
}