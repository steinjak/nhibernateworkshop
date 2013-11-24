using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.MapByCode
{
    public class MappingExceptionsForModel : IMappingExceptions
    {
        public void Apply(ConventionModelMapper mapper)
        {
            mapper.Class<Post>(c => c.Bag(x => x.Media, cm => {}, rel => rel.ManyToMany()));
        }
    }
}