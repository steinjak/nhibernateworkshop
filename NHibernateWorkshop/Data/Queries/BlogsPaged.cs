using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernateWorkshop.Models;

namespace NHibernateWorkshop.Data.Queries
{
    public class BlogsPaged : Query<IEnumerable<Blog>>
    {
        public int Page { get; set; }

        public override IEnumerable<Blog> Execute(ISession session)
        {
            var pageSize = 20;
            var skip = (Page - 1)*pageSize;

            //return session.Query<Blog>().Skip(skip).Take(pageSize).Cacheable();
            return session.Query<Blog>().Skip(skip).Take(pageSize);
        }
    }
}