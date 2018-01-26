using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Tests;
using N2N.Core.Entities;
using N2N.Infrastructure.DataContext;
using N2N.TestData.Helpers;
using NUnit.Framework;


namespace N2N.Infrastructure.Tests
{
    [TestFixture]
    
    public class DataContextTests
    {
        //[Ignore("too slow")]
        [Test]
        public void Should_create_database()
        {
            var ctx = DatabaseDiBootstrapperSQLServer.GetDataContext();

            ctx.N2NUsers.Add(new N2NUser()
            {
                Id = Guid.NewGuid(),
                NickName = "Test",
                Registration = DateTime.Now
            });

            ctx.SaveChanges();

            Assert.IsTrue(ctx.N2NUsers.Any());

            DatabaseDiBootstrapperSQLServer.disposeDataContext(ctx);
        }

        [Test]
        public void Should_get_seeded_in_memoryContext()
        {
            var provider = new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            var ctx = provider.GetService<N2NDataContext>(); 

            Assert.IsTrue(ctx.N2NUsers.Any());
            Assert.IsTrue(ctx.Promises.Any());
            Assert.IsTrue(ctx.PromisesToUsers.Any());
            Assert.IsTrue(ctx.Addresses.Any());
            Assert.IsTrue(ctx.UserAddresseses.Any());
            Assert.IsTrue(ctx.Postcards.Any());
            Assert.IsTrue(ctx.PostcardAddresseses.Any());

        }


        [Test]
        public void Should_get_seeded_sqlContext()
        {
            var provider = new DatabaseDiBootstrapperSQLServer().GetServiceProviderWithSeedDB();
            var ctx = provider.GetService<N2NDataContext>();

            Assert.IsTrue(ctx.N2NUsers.Any());
            Assert.IsTrue(ctx.Promises.Any());
            Assert.IsTrue(ctx.PromisesToUsers.Any());
            Assert.IsTrue(ctx.Addresses.Any());
            Assert.IsTrue(ctx.UserAddresseses.Any());
            Assert.IsTrue(ctx.Postcards.Any());
            Assert.IsTrue(ctx.PostcardAddresseses.Any());

        }
    }
}
