using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NHibernateWorkshop.Models;
using NUnit.Framework;

namespace NHibernateWorkshop.Tests.Queries
{
    [TestFixture]
    public class AdoNetQuerying
    {
        [Test, Explicit]
        public void DoItTheAdoNetWay()
        {
            var localhost = "Data Source=(localdb)\\Projects;Initial Catalog=NHibernateWorkshop;Integrated Security=True";
            var sql = "select b.Id, b.Title, b.OwnerId, u.Username, u.Name " +
                        "from [Blog] b inner join [User] u on u.Id = b.OwnerId " +
                        "where b.Title like @title";

            var blogs = new List<Blog>();
            using (var connection = new SqlConnection(localhost))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (var query = new SqlCommand(sql, connection, transaction))
                    {
                        query.Parameters.Add("@title", SqlDbType.NVarChar, 255).Value = "%and%";
                        using (var results = query.ExecuteReader())
                        {
                            while (results.Read())
                            {
                                blogs.Add(new Blog
                                {
                                    Id = results.GetGuid(results.GetOrdinal("Id")),
                                    Title = results.GetString(results.GetOrdinal("Title")),
                                    Owner = new User
                                    {
                                        Id = results.GetGuid(results.GetOrdinal("OwnerId")),
                                        Username = results.GetString(results.GetOrdinal("Username")),
                                        Name = results.GetString(results.GetOrdinal("Name"))
                                    }
                                });
                            }
                        }
                        transaction.Commit();
                    }
                }
            }

            foreach (var blog in blogs)
            {
                Console.WriteLine("Blog: `{0}' by {1}", blog.Title.Replace('\n', ' '), blog.Owner.Name);
            }
        }
    }
}