using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace NHibernateWorkshop.Tests.Utils
{

    public static class DbManipulationUtilityExtensions
    {
        public static void ConnectToSqlServer(this Configuration cfg, string connectionString)
        {
            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2008Dialect>();
                db.Driver<Sql2008ClientDriver>();
                db.ConnectionString = connectionString;
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.OrderInserts = true;
                db.BatchSize = 100;
                db.PrepareCommands = true;
            });
        }
    }
}