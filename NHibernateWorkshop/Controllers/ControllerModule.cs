using System.Linq;
using System.Web.Mvc;
using Ninject.Modules;
using WebGrease.Css.Extensions;

namespace NHibernateWorkshop.Controllers
{
    public class ControllerModule : NinjectModule
    {
        public override void Load()
        {
            typeof(MvcAction).Assembly.GetExportedTypes()
                .Where(t => typeof(IController).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ForEach(t => Bind<IController>().To(t).Named(t.FullName.ToLowerInvariant()));
        }
    }
}