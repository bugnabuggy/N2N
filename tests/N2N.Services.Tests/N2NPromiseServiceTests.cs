using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Exceptions;
using NUnit.Framework;

namespace N2N.Services.Tests
{

    [TestFixture]
    public class N2NPromiseServiceTests
    {
        [Test]
        public void Should_Throw_Exception_When_User_Adds_Promise_For_Another_UserId()
        {
            var repo = new Mock<IRepository<N2NPromise>>();
            var securSrv = new Mock<ISecurityService>();

            var promise = new N2NPromise(){};

            var promiseSrv =  new N2NPromiseService(repo.Object, securSrv.Object);
            Assert.Throws(typeof(N2NSecurityException), delegate { promiseSrv.Add(promise); });
        } 
    }
}
