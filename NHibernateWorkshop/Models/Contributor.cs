using System;

namespace NHibernateWorkshop.Models
{
    public class Contributor : Entity
    {
        public User User { get; set; }
        public DateTime Since { get; set; }
    }
}