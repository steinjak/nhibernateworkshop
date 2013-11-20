using System.Collections.Generic;

namespace NHibernateWorkshop.Models
{
    public class Blog : Entity
    {
        public User Owner { get; set; }
        public string Title { get;set; }
        public List<Post> Posts { get; set; }
        public List<Contributor> Contributors { get; set; } 
    }
}