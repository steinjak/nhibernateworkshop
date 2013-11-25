using FluentNHibernate.Mapping;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class PostingIdentityMap : ClassMap<PostingIdentity>
    {
        public PostingIdentityMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
        }
    }

    public class UserPostingIdentityMap : SubclassMap<UserPostingIdentity>
    {
        public UserPostingIdentityMap()
        {
            KeyColumn("userpostingidentity_key");
            References(x => x.User);
        }
    }

    public class AnonymousPostingIdentityMap : SubclassMap<AnonymousPostingIdentity>
    {
        public AnonymousPostingIdentityMap()
        {
            KeyColumn("anonymouspostingidentity_key");
            Map(x => x.Nick);
        }
    }
}