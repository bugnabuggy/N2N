using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using NUnit.Framework;


namespace N2N.Infrastructure.Tests
{
    [TestFixture]
    
    public class DataContextTests
    {
        [Ignore("too slow")]
        [Test]
        public void Should_create_database()
        {
            var ctx = Helpers.DataContextHelper.GetDataContext();

            ctx.N2NUsers.Add(new N2NUser()
            {
                Id = Guid.NewGuid(),
                NickName = "Test",
                Registration = DateTime.Now
            });

            ctx.SaveChanges();

            Assert.IsTrue(ctx.N2NUsers.Any());

            Helpers.DataContextHelper.disposeDataContext(ctx);
        }
    }
}
