using System;

namespace NHibernateWorkshop.Models
{
    public class Contributor : Entity
    {
        public virtual User User { get; set; }
        public virtual DateTime Since { get; set; }
    }
}