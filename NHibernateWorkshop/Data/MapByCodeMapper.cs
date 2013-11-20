using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data
{
    public static class MapByCodeMapper
    {
        public static HbmMapping Map()
        {
            var mapper = new ConventionModelMapper();
            SetupConventions(mapper);
            return AutomapAllEntities(mapper);
        }

        private static void SetupConventions(ConventionModelMapper mapper)
        {
            var baseEntityType = typeof (Entity);
            mapper.IsEntity((t, declared) => baseEntityType.IsAssignableFrom(t) && baseEntityType != t && !t.IsInterface);
            mapper.IsRootEntity((t, declared) => baseEntityType == t.BaseType);
        }

        private static HbmMapping AutomapAllEntities(ConventionModelMapper mapper)
        {
            var entities = typeof(Entity).Assembly.GetExportedTypes().Where(t => typeof(Entity).IsAssignableFrom(t) && typeof(Entity) != t);
            return mapper.CompileMappingFor(entities);
        }
    }
}