using System.Collections.Generic;
using System.Web.Mvc;
using NHibernateWorkshop.Data.Queries;
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
}