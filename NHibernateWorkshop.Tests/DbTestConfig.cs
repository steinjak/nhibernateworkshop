using System;
using System.Transactions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NHibernateWorkshop.Data;

namespace NHibernateWorkshop.Tests
{
    public class DbTestConfig
    {
        private readonly static SessionProvider SessionProvider;
        public ISession Session { get; private set; }

        static DbTestConfig()
        {
            Action<Configuration> configDb = cfg => cfg.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.Driver<SQLite20Driver>();
                db.ConnectionProvider<DriverConnectionProvider>();
                db.ConnectionString = @"Data Source=:memory:;Version=3;New=True";
                // important for SQLite in memory, will drop the db on flush/commit otherwise!
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                db.HqlToSqlSubstitutions = @"true=1;false=0;+=||";
            });
            SessionProvider = new SessionProvider(configDb);
        }

        public DbTestConfig()
        {
            Session = SessionProvider.Create();
            var exporter = new SchemaExport(SessionProvider.Configuration);
            exporter.Execute(false, true, false, Session.Connection, null);
        }

        public void Dispose()
        {
            if (Transaction.Current != null)
            {
                // we need to end the ambient transaction scope, if any,
                // else NHibernate will choke on disconnection
                Transaction.Current.Rollback();
                Transaction.Current = null;
            }

            // Session can be closed when testing cleanup code
            // in the infrastructure
            if (Session.IsOpen)
            {
                Session.Close();
            }
        }
    }
}