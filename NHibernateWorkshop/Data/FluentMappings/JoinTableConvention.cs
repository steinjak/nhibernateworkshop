using FluentNHibernate.Conventions.Inspections;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class JoinTableConvention : FluentNHibernate.Conventions.ManyToManyTableNameConvention
    {
        protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
        {
            return collection.Name;
        }

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
        {
            return collection.Name;
        }
    }
}