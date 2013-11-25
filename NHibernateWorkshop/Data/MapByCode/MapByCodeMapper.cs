using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernateWorkshop.Models;
using WebGrease.Css.Extensions;

namespace NHibernateWorkshop.Data.MapByCode
{
    public static class MapByCodeMapper
    {
        public static HbmMapping Map()
        {
            var mapper = new ConventionModelMapper();
            SetupConventions(mapper);
            ApplyExceptions(mapper);
            return AutomapAllEntities(mapper);
        }

        private static void SetupConventions(ConventionModelMapper mapper)
        {
            var baseEntityType = typeof (Entity);
            mapper.IsEntity((t, declared) => baseEntityType.IsAssignableFrom(t) && baseEntityType != t && !t.IsInterface);
            mapper.IsRootEntity((t, declared) => baseEntityType == t.BaseType);

            // Use class name as table name
            mapper.BeforeMapClass += (mi, t, map) => map.Table(t.Name);
            mapper.BeforeMapJoinedSubclass += (mi, t, map) => map.Table(t.Name);
            mapper.BeforeMapUnionSubclass += (mi, t, map) => map.Table(t.Name);

            // Use foreign key conventions and cascade for references/collections
            mapper.BeforeMapManyToOne += (insp, prop, map) =>
            {
                // ex: Message references user in property Sender => Foreign key is named Message.SenderId
                map.Column(prop.LocalMember.Name + "Id");
                map.Cascade(Cascade.Persist);
                UseBackingFieldIfPossible(prop, map);
            };
            mapper.BeforeMapManyToMany += (insp, prop, map) => map.Column(GetNameOfGenericArgumentOfCollection(insp, prop) + "Id");
            mapper.BeforeMapBag += SetupCommonCollectionMappings;
            mapper.BeforeMapSet += SetupCommonCollectionMappings;
            mapper.BeforeMapList += (insp, prop, map) =>
            {
                SetupCommonCollectionMappings(insp, prop, map);
                map.Index(i => i.Column("SequenceNo"));
                map.Inverse(false);  // list end has to be in charge to keep track of index numbers
            };
            mapper.BeforeMapProperty += (insp, prop, map) =>
            {
                // Component name included in property,
                // ex: User has Name(First, Last) => columns are User.NameFirst, User.NameLast
                map.Column(prop.ToColumnName(""));
                // Avoid calling setters with logic in them
                UseBackingFieldIfPossible(prop, map);

                // Use nvarchar(max) for all strings - for simplicity :-)
                if (prop.LocalMember is PropertyInfo && ((PropertyInfo) prop.LocalMember).PropertyType == typeof (string))
                {
                    map.Type(NHibernateUtil.StringClob);
                }
            };

            // Map Id and LastModified for entities
            mapper.Class<Entity>(map => map.Id(x => x.Id, m => m.Generator(Generators.GuidComb)));
        }

        private static void ApplyExceptions(ConventionModelMapper mapper)
        {
            var exceptionDefinitions = typeof(MapByCodeMapper).Assembly.GetExportedTypes()
                .Where(t => typeof(IMappingExceptions).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IMappingExceptions>();
            exceptionDefinitions.ForEach(e => e.Apply(mapper));
        }

        private static void UseBackingFieldIfPossible(PropertyPath prop, IAccessorPropertyMapper map)
        {
            if (!(prop.LocalMember is PropertyInfo)) return;
            // access through backing field, if available, to avoid
            // emitting events when NH hydrates the objects
            var backingField = PropertyToField.GetBackFieldInfo((PropertyInfo)prop.LocalMember);
            if (backingField != null)
            {
                map.Access(Accessor.Field);
            }
        }

        private static void SetupCommonCollectionMappings(IModelInspector insp, PropertyPath prop, ICollectionPropertiesMapper map)
        {
            // ex: Plan has a list of goals => Foreign key is named Goal.PlanId
            map.Key(km => km.Column(prop.GetContainerEntity(insp).Name + "Id"));
            map.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
            var nameOfBackReference = GetNameOfParentProperty(insp, prop);
            if (string.IsNullOrEmpty(nameOfBackReference)) return;
            map.Inverse(true);
            map.Key(km => km.Column(nameOfBackReference + "Id"));
        }

        private static string GetNameOfParentProperty(IModelInspector insp, PropertyPath prop)
        {
            var collectionType = prop.LocalMember.GetPropertyOrFieldType();
            if (!collectionType.IsGenericType) { return null; }
            if (!typeof(IEnumerable<>).IsAssignableFrom(collectionType.GetGenericTypeDefinition())) { return null; }

            var elementType = collectionType.GetGenericArguments().FirstOrDefault(t => typeof(Entity).IsAssignableFrom(t));
            return elementType == null ? null : elementType.GetProperties().Where(p => p.PropertyType == prop.GetContainerEntity(insp)).Select(p => p.Name).FirstOrDefault();
        }

        private static string GetNameOfGenericArgumentOfCollection(IModelInspector insp, PropertyPath prop)
        {
            var collectionType = prop.LocalMember.GetPropertyOrFieldType();
            if (!collectionType.IsGenericType) { return null; }
            if (!typeof(IEnumerable).IsAssignableFrom(collectionType)) { return null; }

            var elementType = collectionType.GetGenericArguments().FirstOrDefault(t => typeof(Entity).IsAssignableFrom(t));
            return elementType == null ? null : elementType.Name;
        }

        private static HbmMapping AutomapAllEntities(ConventionModelMapper mapper)
        {
            var entities = typeof(Entity).Assembly.GetExportedTypes().Where(t => typeof(Entity).IsAssignableFrom(t) && typeof(Entity) != t);
            return mapper.CompileMappingFor(entities);
        }
    }
}