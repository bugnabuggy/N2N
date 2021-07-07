using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Infrastructure.DataContext;
using N2N.TestData.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace N2N.Services.Tests
{
    [TestFixture]
    public class UsersStatisticsServiceTests
    {
        //have to have this and startUp + tearDown to be sure test db will be created and deleted
        private N2NDataContext _ctx;

        [OneTimeSetUp]
        public void StartUp()
        {
            this._ctx = DatabaseDiBootstrapperSqlServer.GetDataContext();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            DatabaseDiBootstrapperSqlServer.DisposeDataContext(_ctx);
        }

        [Test]
        public async Task Should_return_list_of_users_statistics()
        {
            var provider = await new DatabaseDiBootstrapperSqlServer().GetServiceProviderWithSeedDB();
            var usersStatisticsSrv = new UsersStatisticsService(provider.GetService<N2NDataContext>());

            var userStatistics = usersStatisticsSrv.GetUsersStatistics().ToList();

            Assert.IsTrue(userStatistics.Count == 7); //seeded users + 4 tests users
            Assert.IsTrue(userStatistics.Any(x=>x.ToUserPromisesCount > 0));
            Assert.IsTrue(userStatistics.Any(x => x.PromisesCount > 0));
            Assert.IsTrue(userStatistics.Any(x => x.PostcardsCount > 0));
        }
    }
}
