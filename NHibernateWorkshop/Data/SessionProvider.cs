using System;
using NHibernate;
using NHibernate.Cfg;

namespace NHibernateWorkshop.Data
{
    public interface ISessionProvider
    {
        ISession Create();
    }
    public class SessionProvider : ISessionProvider
    {
        private readonly ISessionFactory sessionFactory;
        public Configuration Configuration { get; private set; }

        public SessionProvider(Action<Configuration> configDb = null)
        {
            Configuration = new Configuration();
            (configDb ?? (c => c.Configure()))(Configuration);
            Configuration.AddDeserializedMapping(MapByCodeMapper.Map(), "Model");
            Configuration.DataBaseIntegration(db => db.OrderInserts = true);
            sessionFactory = Configuration.BuildSessionFactory();
        }

        public ISession Create()
        {
            return sessionFactory.OpenSession();
        }
    }
}