using System;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernateWorkshop.Data.FluentMappings;
using NHibernateWorkshop.Data.MapByCode;

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
            Configuration = ConfigureUsingMappingByCode();
            Configuration.DataBaseIntegration(db => db.OrderInserts = true);
            (configDb ?? (c => c.Configure()))(Configuration);
            sessionFactory = Configuration.BuildSessionFactory();
        }

        private Configuration ConfigureUsingMappingByCode()
        {
            var cfg = new Configuration();
            cfg.AddDeserializedMapping(MapByCodeMapper.Map(), "Model");
            return cfg;
        }

        private Configuration ConfigureUsingFluentNHibernate()
        {
            return Fluently.Configure()
                .Mappings(x => x
                    .FluentMappings.AddFromAssemblyOf<UserMap>()
                    .Conventions.AddFromAssemblyOf<ForeignKeyConvention>())
                .BuildConfiguration();
        }

        public ISession Create()
        {
            return sessionFactory.OpenSession();
        }
    }
}