using System.Web.Mvc;

namespace NHibernateWorkshop.Controllers.Home
{
    public class Index : MvcAction
    {
        public ActionResult Get()
        {
            return View();
        }
    }
}