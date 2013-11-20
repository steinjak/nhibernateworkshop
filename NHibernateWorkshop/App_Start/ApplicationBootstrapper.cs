using System.Linq;
using System.Web.Mvc;
using NHibernateWorkshop.Controllers;
using NHibernateWorkshop.Data;
using Ninject;

namespace NHibernateWorkshop
{
    public static class ApplicationBootstrapper
    {
        public static void Configure()
        {
            var kernel = new StandardKernel(new ControllerModule(), new DbModule());
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel, "NHibernateWorkshop.Controllers"));
        }

        private class NinjectControllerFactory : DefaultControllerFactory
        {
            private readonly IKernel kernel;
            private readonly string baseNamespace;

            public NinjectControllerFactory(IKernel kernel, string baseNamespace)
            {
                this.kernel = kernel;
                this.baseNamespace = baseNamespace.ToLowerInvariant();
            }

            public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
            {
                var area = requestContext.RouteData.Values["area"] as string ?? string.Empty;
                var action = requestContext.RouteData.Values["action"] as string;
                return TryGetController(ComponentNameFromParts(area, controllerName, action))
                    ?? TryGetController(ComponentNameFromParts(controllerName, action))
                    ?? TryGetController(ComponentNameFromParts(area, controllerName + "Controller"))
                    ?? TryGetController(ComponentNameFromParts(controllerName + "Controller"));
            }

            private string ComponentNameFromParts(params string[] parts)
            {
                return baseNamespace + "." + string.Join(".", parts.Select(p => p.ToLowerInvariant()).ToArray());
            }

            private IController TryGetController(string componentName)
            {
                return kernel.TryGet<IController>(componentName);
            }
        }
    }
}