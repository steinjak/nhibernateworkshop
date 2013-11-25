using System.Collections;
using NHibernate;
using NHibernateWorkshop.Data;
using NHibernateWorkshop.Tests.Controllers;
using NUnit.Framework;
using Plant.Core;

namespace NHibernateWorkshop.Tests
{
    public abstract class TestBase
    {
        static TestBase()
        {
            Plant = new BasePlant().WithBlueprintsFromAssemblyOf<ControllerTest>();
        }

        protected static BasePlant Plant { get; private set; }
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

        protected void DatabaseContaining(params object[] entities)
        {
            SaveAll(entities);
            Session.Flush();
            Session.Clear();
        }

        private void SaveAll(IEnumerable entities)
        {
            foreach (var entity in entities)
            {
                var enumerable = entity as IEnumerable;
                if (enumerable != null)
                {
                    SaveAll(enumerable);
                }
                else
                {
                    Session.Save(entity);
                }
            }
        }

        protected class TestSessionProvider : ISessionProvider
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