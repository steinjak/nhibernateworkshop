using System.Collections.Generic;

namespace NHibernateWorkshop.Models
{
    public class Blog : Entity
    {
        public virtual User Owner { get; set; }
        public virtual string Title { get; set; }
        public virtual IList<Post> Posts { get; set; }
        public virtual IList<Contributor> Contributors { get; set; } 
    }
}