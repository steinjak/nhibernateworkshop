using System;
using System.Linq;
using System.Web.Mvc;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Controllers.Blogs
{
    public class View : MvcAction
    {
        public ActionResult Get(Guid id)
        {
            var blog = Db.Load<Blog>(id);
            return View(new ViewBlogModel
            {
                Blog = blog,
                Posts = blog.Posts.Where(p => p.PublishedOn.HasValue).OrderByDescending(p => p.PublishedOn).Take(15).ToArray(),
                FeaturedPosts = blog.Posts.Where(p => p.IsFeatured && p.PublishedOn.HasValue).OrderByDescending(p => p.PublishedOn).Take(3).ToArray()
            });
        }
    }

    public class ViewBlogModel
    {
        public Blog Blog { get; set; }
        public Post[] Posts { get; set; }
        public Post[] FeaturedPosts { get; set; }
    }
}