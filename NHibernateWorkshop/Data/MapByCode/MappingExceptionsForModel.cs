using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.MapByCode
{
    public class MappingExceptionsForModel : IMappingExceptions
    {
        public void Apply(ConventionModelMapper mapper)
        {
            mapper.Class<Post>(p => p.Property(x => x.Content, o => o.Type(NHibernateUtil.StringClob)));
            mapper.Class<Comment>(p => p.Property(x => x.Text, o => o.Type(NHibernateUtil.StringClob)));
        }
    }
}