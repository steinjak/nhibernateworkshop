using System.Web;
using System.Web.Mvc;

namespace NHibernateWorkshop.Controllers
{
    public static class UrlExtensions
    {
        public static string MvcAction<TAction>(this UrlHelper url, object routeValues = null)
        {
            var info = new MvcActionInfo(typeof(TAction));
            return url.Action(info.Action, info.Controller, routeValues);
        }

        public static string External(this UrlHelper url, string baseUri, string relativeUri)
        {
            var relativePath = VirtualPathUtility.ToAppRelative(relativeUri).Substring(2);

            if (!baseUri.EndsWith("/"))
            {
                return baseUri + "/" + relativePath;
            }

            return baseUri + relativePath;
        }
    }
}