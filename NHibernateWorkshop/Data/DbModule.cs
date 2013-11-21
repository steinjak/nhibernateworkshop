using System.Web;
using Ninject.Modules;

namespace NHibernateWorkshop.Data
{
    public class DbModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionProvider>().ToConstant(new SessionProvider());
            Bind<SessionManager>().ToSelf().InScope(c => HttpContext.Current);
        }
    }
}