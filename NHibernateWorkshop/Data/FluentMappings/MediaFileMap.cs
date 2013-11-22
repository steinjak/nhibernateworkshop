using FluentNHibernate.Mapping;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class MediaFileMap : ClassMap<MediaFile>
    {
        public MediaFileMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name);
            Map(x => x.Url);
            References(x => x.Owner);
        }
    }
}