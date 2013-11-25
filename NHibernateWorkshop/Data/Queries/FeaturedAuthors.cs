using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.Queries
{
    public class FeaturedAuthors : Query<IEnumerable<UserAndFeaturedPost>>
    {
        public override IEnumerable<UserAndFeaturedPost> Execute(ISession session)
        {
            var blogs = session.Query<Blog>().OrderByDescending(b => b.Posts.Count()).Take(3).ToArray();
            var ids = blogs.Select(b => b.Id).ToArray();/*
            var featured = session
                .CreateSQLQuery("SELECT {p.*} FROM Posts p WHERE p.BlogId in (:blogIds) and p.IsFeatured = 1")
                .AddEntity("p", typeof(Post))
                .SetParameterList("blogIds", ids)
                .List<Post>();*/
            return blogs.Select(b => new UserAndFeaturedPost {User = b.Owner, Post = null});
        }
    }

    public class UserAndFeaturedPost
    {
        public User User { get; set; }
        public Post Post { get; set; }
    }
}