using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NHibernateWorkshop
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                Startup();
            }
            catch (Exception)
            {
                HttpRuntime.UnloadAppDomain();  // make sure app reloads after failure in startup
                throw;
            }
        }

        private static void Startup()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApplicationBootstrapper.Configure();
        }
    }
}
