using System;
using System.Collections.Generic;

namespace NHibernateWorkshop.Models
{
    public class Post : Entity
    {
        public virtual User Author { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual DateTime? PublishedOn { get; set; }
        public virtual bool IsSticky { get; set; }
        public virtual bool IsFeatured { get; set; }
        public virtual MediaFile FeaturedImage { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<MediaFile> Media { get; set; }
    }
}