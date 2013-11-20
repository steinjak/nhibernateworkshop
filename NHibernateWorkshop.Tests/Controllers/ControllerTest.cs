using NHibernate;
using NHibernateWorkshop.Controllers;
using NHibernateWorkshop.Data;
using NUnit.Framework;

namespace NHibernateWorkshop.Tests.Controllers
{
    public abstract class ControllerTest
    {
        private DbTestConfig NhConfig { get; set; }
        protected ISession Session { get { return NhConfig.Session; } }

        [SetUp]
        public void EmptyDb()
        {
            NhConfig = new DbTestConfig();
        }

        [TearDown]
        public void DropSqlDatabase()
        {
            if (NhConfig == null) { return; }
            NhConfig.Dispose();
            NhConfig = null;
        }

        public T Init<T>() where T : MvcAction, new()
        {
            var instance = new T();
            instance.SessionManager = new SessionManager(new TestSessionProvider(Session));
            return instance;
        }

        private class TestSessionProvider : ISessionProvider
        {
            private readonly ISession session;

            public TestSessionProvider(ISession session)
            {
                this.session = session;
            }

            public ISession Create()
            {
                return session;
            }
        }
    }
}