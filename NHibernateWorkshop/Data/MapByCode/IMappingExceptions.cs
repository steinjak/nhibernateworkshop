using NHibernate.Mapping.ByCode;

namespace NHibernateWorkshop.Data.MapByCode
{
    internal interface IMappingExceptions
    {
        void Apply(ConventionModelMapper mapper);
    }
}