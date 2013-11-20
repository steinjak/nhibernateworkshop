namespace NHibernateWorkshop.Models
{
    public class MediaFile : Entity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public User Owner { get; set; }
    }
}