using System;
using System.Web.Mvc;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Controllers.Posts
{
    public class Read : MvcAction
    {
        public ActionResult Get(Guid id)
        {
            return View(new ReadPostModel
            {
                Post = Db.Load<Post>(id)
            });
        }
    }

    public class ReadPostModel
    {
        public Post Post { get; set; }
    }
}