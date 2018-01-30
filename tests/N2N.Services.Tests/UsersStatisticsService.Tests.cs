using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Api.Tests;
using N2N.Infrastructure.DataContext;
using N2N.TestData.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace N2N.Services.Tests
{
    [TestFixture]
    public class UsersStatisticsServiceTests
    {
        private N2NDataContext ctx = null;
        [OneTimeSetUp]
        public void Setup()
        {
            //ctx = DatabaseDiBootstrapperSQLServer.GetDataContext();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            //DatabaseDiBootstrapperSQLServer.DisposeDataContext(ctx);
        }

        [Test]
        public void Should_return_list_of_users_statistics()
        {
            var context = new DatabaseDiBootstrapperInMemory().GetDataContext();

            var usersStatisticsSrv = new UsersStatisticsService(context);

            var userStatistics = usersStatisticsSrv.GetUsersStatistics();

            Assert.IsTrue(userStatistics.Count() == 4);

        }
    }
}
