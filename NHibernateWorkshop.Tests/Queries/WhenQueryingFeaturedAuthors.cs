using System;
using System.Collections.Generic;
using NHibernateWorkshop.Data.Queries;
using NHibernateWorkshop.Models;
using NUnit.Framework;
using System.Linq;

namespace NHibernateWorkshop.Tests.Queries
{
    [TestFixture]
    public class WhenQueryingFeaturedAuthors : TestBase
    {
        private IEnumerable<UserAndFeaturedPost> result;

        [SetUp]
        public void Given()
        {
            var authors = Enumerable.Range(0, 10).Select(i => Plant.Create<User>(new {Username = string.Format("user-{0}", i)})).ToArray();
            var blogs = authors.Select((a, i) => new Blog
            {
                Owner = a,
                Posts = Enumerable.Range(1, i+1).Select(n => new Post {Author = a, IsFeatured = i == 1}).ToList()
            });
            DatabaseContaining(authors, blogs);

            var query = new FeaturedAuthors();
            result = query.Execute(Session);
        }

        [Test]
        public void TheThreeUsersWithTheMostTotalPostsAreReturned()
        {
            Assert.That(result.Select(r => r.User.Username), Is.EquivalentTo(new[]{"user-9", "user-8", "user-7"}));
        }

        [Test, Ignore("Not finished")]
        public void EachUserHasAFeaturedPost()
        {
            Assert.That(result.All(r => r.Post.IsFeatured));
        }
    }
}