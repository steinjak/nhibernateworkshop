using System;
using System.Collections.Generic;

namespace NHibernateWorkshop.Models
{
    public class Post : Entity
    {
        public Blog Blog { get; set; }
        public DateTime? PublishedOn { get; set; }
        public bool IsSticky { get; set; }
        public bool IsFeatured { get; set; }
        public MediaFile FeaturedImage { get; set; }
        public string Title { get;set; }
        public string Content { get;set; }
        public List<Comment> Comments { get; set; }
    }
}