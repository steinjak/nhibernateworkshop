using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using NHibernateWorkshop.Data;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Controllers.Blogs
{
    public class Index : MvcAction
    {
        public ActionResult Get(int? page)
        {
            var actualPage = page ?? 1;
            return View(new BlogIndexModel
            {
                Blogs = Db.Query(new BlogsPaged {Page = actualPage}),
                Page = actualPage,
                UrlToNextPage = Url.Action("Index", new { page = actualPage + 1 }),
                UrlToPreviousPage = actualPage > 1 ? Url.Action("Index", new { page = actualPage - 1 }) : string.Empty
            });
        }
    }

    public class BlogIndexModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public int Page { get; set; }
        public string UrlToNextPage { get; set; }
        public string UrlToPreviousPage { get; set; }
    }

    public class BlogsPaged : Query<IEnumerable<Blog>>
    {
        public int Page { get; set; }

        public override IEnumerable<Blog> Execute(ISession session)
        {
            var pageSize = 20;
            var skip = (Page - 1)*pageSize;

            //return session.Query<Blog>().Skip(skip).Take(pageSize).Cacheable();
            return session.Query<Blog>().Skip(skip).Take(pageSize);
        }
    }
}