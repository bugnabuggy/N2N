﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using N2N.Core.Entities;
using N2N.Infrastructure.DataContext;
using N2N.TestData;
using N2N.TestData.Helpers;
using NUnit.Framework;


namespace N2N.Infrastructure.Tests
{
    [TestFixture]
    
    public class DataContextTests
    {
        private N2NDataContext _ctx = null;

        [OneTimeSetUp]
        public void Startup()
        {
            _ctx = DatabaseDiBootstrapperSqlServer.GetDataContext();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            DatabaseDiBootstrapperSqlServer.DisposeDataContext(_ctx);
        }

        //[Ignore("too slow")]
        [Test]
        public void Should_create_database()
        {
            var ctx = DatabaseDiBootstrapperSqlServer.GetDataContext();

            ctx.N2NUsers.Add(new N2NUser()
            {
                Id = Guid.NewGuid(),
                NickName = "Test",
                Registration = DateTime.Now
            });

            ctx.SaveChanges();

            Assert.IsTrue(ctx.N2NUsers.Any());

            DatabaseDiBootstrapperSqlServer.DisposeDataContext(ctx);
        }

        private void CheckContext(N2NDataContext ctx)
        {
            Assert.IsTrue(ctx.N2NUsers.Any());
            Assert.IsTrue(ctx.Promises.Any());
            Assert.IsTrue(ctx.PromisesToUsers.Any());
            Assert.IsTrue(ctx.Addresses.Any());
            Assert.IsTrue(ctx.UserAddresseses.Any());
            Assert.IsTrue(ctx.Postcards.Any());
            Assert.IsTrue(ctx.PostcardAddresseses.Any());
        }

        [Test]
        public async Task Should_get_seeded_in_memoryContext()
        {
            var provider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            var ctx = provider.GetService<N2NDataContext>();

            CheckContext(ctx);
        }


        [Test]
        public async Task Should_get_seeded_sqlContext()
        {
            var provider = await new DatabaseDiBootstrapperSqlServer().GetServiceProviderWithSeedDB();
            var ctx = provider.GetService<N2NDataContext>();

            CheckContext(ctx);
        }

        [Test]
        public void Should_add_addresses_to_test_database()
        {
            var user = N2NUsersList.GetNotInDbUser();
            var inMemoryContext = new DatabaseDiBootstrapperInMemory().GetDataContext();
            var initializer = new TestDbContextInitializer();

            inMemoryContext.N2NUsers.Add(user);
            initializer.AddAddressess(user, inMemoryContext);
            inMemoryContext.SaveChanges();

            Assert.IsTrue(inMemoryContext.Addresses.Any());
        }

        
    }
}
