using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate;
using NHibernate.Cfg;
using NHibernateWorkshop.Data;
using NHibernateWorkshop.Data.MapByCode;
using NHibernateWorkshop.Models;
using NUnit.Framework;
using Plant.Core;

namespace NHibernateWorkshop.Tests.Utils
{
    [TestFixture, Explicit]
    public class GenerateData
    {
        private ISessionFactory sessionFactory;
        private const int NumberOfUsers = 5000;
        private const int NumberOfContributors = 3;
        private const int NumberOfPostsPerUser = 350;
        private const int NumberOfCommentsPerPost = 50;

        private readonly Random random = new Random();
        private Stopwatch sw;

        public GenerateData()
        {
            var localhost = "Data Source=(localdb)\\Projects;Initial Catalog=NHibernateWorkshop;Integrated Security=True";

            var cfg = new Configuration();
            cfg.ConnectToSqlServer(localhost);
            cfg.AddDeserializedMapping(MapByCodeMapper.Map(), "Model");
            cfg.SetProperty("adonet.batch_size", "100");
            sessionFactory = cfg.BuildSessionFactory();

            sw = new Stopwatch();
        }

        [Test, Explicit]
        public void Generate()
        {
            Console.WriteLine("Generating data...");
            sw.Start();
            var users = CreateUsers();
            Console.WriteLine("Generated {0} users in {1}, continuing to blogs...", users.Length, sw.Elapsed);
            GenerateBlogsForAll(users);
            sw.Stop();
            Console.WriteLine("Finished in {0}", sw.Elapsed);
        }

        private User[] CreateUsers()
        {
            var plant = new BasePlant().WithBlueprintsFromAssemblyOf<GenerateData>();
            var users = Enumerable.Range(0, NumberOfUsers).Select(n => plant.Create<User>()).ToArray();
            using (var session = sessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var user in users)
                {
                    session.Insert(user);
                }
                tx.Commit();
                session.Close();
            }
            return users;
        }

        private void GenerateBlogsForAll(User[] users)
        {
            var blogs = new List<Blog>();
            foreach (var user in users)
            {
                blogs.Add(GenerateBlogFor(user, users));

                if (blogs.Count % 100 == 0)
                {
                    SaveAll(blogs);
                    blogs.Clear();
                }
            }
            SaveAll(blogs);
        }

        private void SaveAll(IList<Blog> blogs)
        {
            var start = sw.Elapsed;
            Console.Write("Saving {0} blog(s) to the DB...", blogs.Count);
            using (var session = sessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var blog in blogs)
                {
                    session.Insert(blog);
                }
                tx.Commit();
                session.Close();
            }
            Console.WriteLine("transaction committed in {0}; total time = {1}", sw.Elapsed - start, sw.Elapsed);
        }

        private Blog GenerateBlogFor(User user, User[] users)
        {
            var theme = Theme.PickOne();
            var contributors = PickContributorsFrom(users);
            var blog = new Blog
            {
                Owner = user,
                Contributors = contributors,
                Title = theme.GenerateTitle()
            };
            blog.Posts = GeneratePosts(blog, theme, users);
            return blog;
        }

        private List<Post> GeneratePosts(Blog blog, Theme theme, User[] users)
        {
            var posts = new List<Post>();
            var numberOfPosts = random.Next(NumberOfPostsPerUser);
            for (int i = 0; i < numberOfPosts; i++)
            {
                var post = new Post
                {
                    Blog = blog,
                    Author = PickAuthor(blog),
                    IsFeatured = random.Next(100) < 3,
                    IsSticky = random.Next(100) == 0,
                    Title = theme.GeneratePostTitle(),
                    Content = theme.GeneratePostContent(),
                    FeaturedImage = theme.GenerateMedia(blog.Owner),
                    PublishedOn = random.Next(100) < 95 ? GenerateDate() : (DateTime?)null,
                    Media = Enumerable.Range(0, random.Next(7)).Select(x => theme.GenerateMedia(blog.Owner)).ToList()
                };
                if (post.PublishedOn.HasValue)
                {
                    post.Comments = GenerateComments(post, theme, users);
                }
                posts.Add(post);
            }
            return posts;
        }

        private List<Comment> GenerateComments(Post post, Theme theme, User[] users)
        {
            var comments = new List<Comment>();
            var numberOfComments = random.Next(NumberOfCommentsPerPost/3, NumberOfCommentsPerPost);
            for (int i = 0; i < numberOfComments; i++)
            {
                comments.Add(new Comment
                {
                    Author = random.Next(100) < 70 ? (PostingIdentity)new UserPostingIdentity{User = PickAUser(users)} : new AnonymousPostingIdentity {Nick = "Anonymous Coward"},
                    Text = theme.GenerateComment(),
                    Timestamp = GenerateDate(post.PublishedOn)
                });
            }
            return comments;
        }

        private DateTime GenerateDate(DateTime? after = null)
        {
            if (after.HasValue)
            {
                return after.Value.AddDays(random.Next((int)(DateTime.Now - after.Value).TotalDays));
            }
            return DateTime.Now.Subtract(TimeSpan.FromDays(random.Next(5 * 365)));
        }

        private User PickAuthor(Blog blog)
        {
            if (random.Next(100) < 85 || !blog.Contributors.Any())
            {
                return blog.Owner;
            }
            return blog.Contributors[random.Next(blog.Contributors.Count)].User;
        }

        private List<Contributor> PickContributorsFrom(User[] users)
        {
            var result = new List<Contributor>();
            for (int i = 0; i < NumberOfContributors; i++)
            {
                result.Add(new Contributor
                {
                    Since = DateTime.Now,
                    User = PickAUser(users)
                });
            }
            return result;
        }

        private User PickAUser(User[] users)
        {
            return users[random.Next(users.Length)];
        }

        private class Theme
        {
            private static readonly Theme[] Themes = new[]
            {
                new Theme("crime", "cats"), 
                new Theme("kronikk", "city"), 
                new Theme("psycho", "people"), 
                new Theme("speech", "technics"),
                new Theme("verge", "business"), 
            };

            private static readonly Random Random = new Random();
            private static readonly Regex MatchSentences = new Regex(@"[\.\?!]\s*([A-Z].*[\.\?!])", RegexOptions.Compiled);
            private readonly string text;
            private readonly string tags;
            private int imageNumber = 1;

            public static Theme PickOne()
            {
                return Themes[Random.Next(Themes.Length)];
            }

            private Theme(string name, string tags)
            {
                this.tags = tags;
                text = File.ReadAllText("Utils\\Text\\" + name + ".txt");
            }

            public string GenerateTitle()
            {
                return RandomSentence(7);
            }

            public string GenerateComment()
            {
                return GetExcerpt(150, 500);
            }

            public string GeneratePostTitle()
            {
                return RandomSentence(14);
            }

            public string GeneratePostContent()
            {
                return GetExcerpt(1000, 7000);
            }

            public MediaFile GenerateMedia(User owner)
            {
                //string.Format("http://flickrholdr.com/390/197/{0}/{1}", tags, imageNumber++)
                return new MediaFile
                {
                    Name = RandomSentence() + ".jpg",
                    Owner = owner,
                    Url = string.Format("http://lorempixel.com/390/197/{0}/{1}", tags, (imageNumber++ % 10) +1)
                };
            }

            private string RandomSentence(int maxWords = 7)
            {
                var excerpt = GetExcerpt(300, 500);
                var match = MatchSentences.Match(excerpt);
                return FirstWordsOf(match.Success ? match.Groups[1].Captures[0].Value : excerpt.Substring(21), maxWords);
            }

            private string FirstWordsOf(string excerpt, int num = 7)
            {
                var words = excerpt.Split(' ');
                return string.Join(" ", words.Take(Math.Min(num, words.Length)));
            }

            private string GetExcerpt(int min, int max)
            {
                return text.Substring(Random.Next(text.Length - max), Random.Next(min, max));
            }
        }
    }
}