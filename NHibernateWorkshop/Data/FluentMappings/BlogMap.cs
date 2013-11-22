using FluentNHibernate.Mapping;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class BlogMap : ClassMap<Blog>
    {
        public BlogMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title);
            References(x => x.Owner);
            HasMany(x => x.Contributors);
            HasMany(x => x.Posts);
        }
    }
}