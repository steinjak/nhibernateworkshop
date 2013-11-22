using FluentNHibernate.Mapping;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class ContributorMap : ClassMap<Contributor>
    {
        public ContributorMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Since);
            References(x => x.User);
        }
    }
}