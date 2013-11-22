using FluentNHibernate.Mapping;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class PostMap : ClassMap<Post>
    {
        public PostMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title);
            Map(x => x.Content);
            Map(x => x.PublishedOn);
            Map(x => x.IsSticky);
            Map(x => x.IsFeatured);
            References(x => x.Author);
            References(x => x.FeaturedImage);
            HasMany(x => x.Comments);
            HasManyToMany(x => x.Media);
        }
    }
}