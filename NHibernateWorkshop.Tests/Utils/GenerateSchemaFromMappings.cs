using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NHibernateWorkshop.Data.MapByCode;
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
            cfg.AddDeserializedMapping(MapByCodeMapper.Map(), "Model");

            var exporter = new SchemaExport(cfg);
            exporter.Execute(true, false, false, null, null);  // dump schema to console
        }

        [Test, Explicit]
        public void InstallSchemaFromMappingsToDb()
        {
            var localhost = "Data Source=(localdb)\\Projects;Initial Catalog=NHibernateWorkshop;Integrated Security=True";

            var cfg = new Configuration();
            cfg.ConnectToSqlServer(localhost);
            cfg.AddDeserializedMapping(MapByCodeMapper.Map(), "Model");
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
            var map = MapByCodeMapper.Map();
            var serializer = new XmlSerializer(map.GetType());

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
            {
                serializer.Serialize(writer, map);
            }

            Console.WriteLine(sb.ToString());
        }
    }
}