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
                Posts = blog.Posts.Take(15).ToArray()
            });
        }
    }

    public class ViewBlogModel
    {
        public Blog Blog { get; set; }
        public Post[] Posts { get; set; }
    }
}