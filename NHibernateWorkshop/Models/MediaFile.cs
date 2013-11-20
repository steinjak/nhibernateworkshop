namespace NHibernateWorkshop.Models
{
    public class MediaFile : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual User Owner { get; set; }
    }
}