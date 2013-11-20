using System;

namespace NHibernateWorkshop.Models
{
    public class Comment : Entity
    {
        public Post CommentTo { get; set; }
        public PostingIdentity Author { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }
    }
}