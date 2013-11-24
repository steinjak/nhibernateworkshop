using FluentNHibernate.Mapping;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Timestamp);
            Map(x => x.Text);
            References(x => x.Author);
        }
    }
}