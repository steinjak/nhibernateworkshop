using System.Collections.Generic;
using NHibernate.Linq;
using NHibernateWorkshop.Controllers.Blogs;
using NHibernateWorkshop.Data.Queries;
using NHibernateWorkshop.Models;
using NHibernateWorkshop.Tests.Controllers;
using NUnit.Framework;
using System.Linq;

namespace NHibernateWorkshop.Tests.Queries
{
    [TestFixture]
    public class WhenQueryingBlogsByPage : ControllerTest
    {
        private IEnumerable<Blog> result;

        [SetUp]
        public void Given()
        {
            var blogs = Enumerable.Range(0, 100).Select(n => new Blog {Owner = Plant.Create<User>()});
            blogs.ForEach(blog => Session.Save(blog));
            Session.Flush();

            result = new BlogsPaged {Page = 2}.Execute(Session);
        }

        [Test]
        public void OnlyOnePageOfBlogsIsReturned()
        {
            Assert.That(result.Count(), Is.EqualTo(20));
        }
    }
}