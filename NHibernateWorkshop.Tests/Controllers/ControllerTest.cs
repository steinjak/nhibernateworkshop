using NHibernateWorkshop.Controllers;
using NHibernateWorkshop.Data;

namespace NHibernateWorkshop.Tests.Controllers
{
    public abstract class ControllerTest : TestBase
    {
        public T Init<T>() where T : MvcAction, new()
        {
            var instance = new T();
            instance.SessionManager = new SessionManager(new TestSessionProvider(Session));
            return instance;
        }
    }
}