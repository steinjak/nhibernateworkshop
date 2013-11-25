using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using NHibernateWorkshop.Data;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Controllers.Blogs
{
    public class View : MvcAction
    {
        public ActionResult Get(Guid id)
        {
            var model = Db.Query(new GetAllPostsInBlog {BlogId = id});
            return View(model);
        }
    }

    public class GetAllPostsInBlog : Query<ViewBlogModel>
    {
        public Guid BlogId { get; set; }
        public override ViewBlogModel Execute(ISession session)
        {
            var blog = session.Query<Blog>()
                .FetchMany(b => b.Contributors).ThenFetch(c => c.User)
                .Fetch(b => b.Owner)
                .Where(b => b.Id == BlogId)
                .ToFutureValue();
            var posts = session.Query<Post>()
                    .Fetch(p => p.FeaturedImage)
                    .Where(p => p.Blog.Id == BlogId && p.PublishedOn.HasValue)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(15)
                    .ToFuture();
            var featured = session.Query<Post>()
                    .Fetch(p => p.FeaturedImage)
                    .Where(p => p.Blog.Id == BlogId && p.PublishedOn.HasValue && p.IsFeatured)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(3)
                    .ToFuture();
            return new ViewBlogModel
            {
                Blog = blog.Value,
                Posts = posts.ToArray(),
                FeaturedPosts = featured.ToArray()
            };
        }
    }

    public class ViewBlogModel
    {
        public Blog Blog { get; set; }
        public Post[] Posts { get; set; }
        public Post[] FeaturedPosts { get; set; }
    }
}