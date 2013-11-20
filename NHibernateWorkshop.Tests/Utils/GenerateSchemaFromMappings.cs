using System;
using System.Xml.Serialization;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernateWorkshop.Tests.Utils
{
    [TestFixture, Explicit]
    public class GenerateSchemaFromMappings
    {
        [Test, Explicit]
        public void GenerateDbSchemaForSqlServer()
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2008Dialect>();
                db.Driver<Sql2008ClientDriver>();
            });
            cfg.AddDeserializedMapping(DbConfig.MapByCodeMapper.Map(), "Model");

            var exporter = new SchemaExport(cfg);
            exporter.Execute(true, false, false, null, null);  // dump schema to console
        }

        [Test, Explicit]
        public void InstallSchemaFromMappingsToDb()
        {
            var localhost = "Data Source=(localdb)\\Projects;Initial Catalog=NHibernateWorkshop;Integrated Security=True";

            var cfg = new Configuration();
            cfg.ConnectToSqlServer(localhost);
            cfg.AddDeserializedMapping(DbConfig.MapByCodeMapper.Map(), "Model");
            var sessionFactory = cfg.BuildSessionFactory();

            var exporter = new SchemaExport(cfg);
            using (var session = sessionFactory.OpenSession())
            {
                exporter.Execute(false, true, false, session.Connection, null);
                session.Close();
            }
        }

        [Test, Explicit]
        public void DumpMappingsToConsole()
        {
            var map = DbConfig.MapByCodeMapper.Map();
            var ser = new XmlSerializer(map.GetType());
            ser.Serialize(Console.Out, map);
        }
    }
}