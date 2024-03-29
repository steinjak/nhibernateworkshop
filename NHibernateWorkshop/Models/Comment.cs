﻿using System;

namespace NHibernateWorkshop.Models
{
    public class Comment : Entity
    {
        public virtual PostingIdentity Author { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual string Text { get; set; }
    }
}