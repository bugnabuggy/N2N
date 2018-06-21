using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using N2N.Core.Entities;
using N2N.Core.Services;
using N2N.Data.Repositories;
using N2N.Infrastructure.Exceptions;
using NUnit.Framework;

namespace N2N.Services.Tests
{

    [TestFixture]
    public class N2NPromiseServiceTests
    {
        private Mock<ISecurityService> _securSrv;
        private Mock<ISecurityService> _adminSecurSrv;

        private Mock<IRepository<N2NPromise>> _repo;

        [SetUp]
        public void Start()
        {
            _securSrv = new Mock<ISecurityService>();
            _securSrv.Setup(x => x.GetCurrentN2NUserId()).Returns(Guid.Empty);

            //pretend that has access as admin
            _adminSecurSrv = new Mock<ISecurityService>();
            _adminSecurSrv.Setup(x => x.GetCurrentN2NUserId()).Returns(Guid.Empty);
            _adminSecurSrv.Setup(x => x.HasAccess()).Returns(true);

            _repo = new Mock<IRepository<N2NPromise>>();
            _repo.Setup(x=>x.Data).Returns(new List<N2NPromise>()
            {
                new N2NPromise(){N2NUserId = Guid.NewGuid()}
            }.AsQueryable());
            _repo.Setup(x => x.Add(It.IsAny<N2NPromise>())).Returns(new N2NPromise());
            _repo.Setup(x => x.Update(It.IsAny<N2NPromise>())).Returns((N2NPromise x) => x );
            _repo.Setup(x => x.Delete(It.IsAny<N2NPromise>())).Returns((N2NPromise x) => x );

        }

        [Test]
        public void Should_add_promise_when_userId_same_as_in_promise()
        {
            N2NPromise promiseSpy = null;
            var repo = new Mock<IRepository<N2NPromise>>();
            repo.Setup(x => x.Add(It.IsAny<N2NPromise>()))
                .Returns((N2NPromise x) =>
                {
                    promiseSpy = x;
                    return x ; 
                });

            var promise = new N2NPromise() { N2NUserId = Guid.Empty, Text = "Tests"};

            var promiseSrv = new N2NPromiseService(repo.Object, _securSrv.Object);
            promiseSrv.Add(promise);


            Assert.AreEqual(promise, promiseSpy);
            Assert.AreEqual(promise.N2NUserId, promiseSpy.N2NUserId);
            Assert.AreEqual(promise.Text, promiseSpy.Text);
        }

        [Test]
        public void Should_Throw_Exception_When_User_Adds_Promise_For_Another_UserId()
        {
            var promise = new N2NPromise(){ N2NUserId = Guid.NewGuid()};

            var promiseSrv =  new N2NPromiseService(_repo.Object, _securSrv.Object);
            Assert.Throws(typeof(N2NSecurityException), delegate { promiseSrv.Add(promise); });
        }


        [Test]
        public void Should_Throw_Exception_When_User_Edit_Promise_For_Another_UserId()
        {
            var promise = new N2NPromise() { N2NUserId = Guid.NewGuid() };

            var promiseSrv = new N2NPromiseService(_repo.Object, _securSrv.Object);
            Assert.Throws(typeof(N2NSecurityException), delegate { promiseSrv.Update(promise); });
        }


        [Test]
        public void Should_Edit_Promise_For_Another_UserId_If_Admin()
        {
            var promise = new N2NPromise() { N2NUserId = Guid.NewGuid() };
            var promiseSrv = new N2NPromiseService(_repo.Object, _adminSecurSrv.Object);

            var editedPromise = promiseSrv.Update(promise);

            Assert.AreEqual(editedPromise, promise);

        }


        [Test]
        public void Should_Throw_Exception_When_User_Delete_Promise_For_Another_UserId()
        {
            var promise = new N2NPromise() { N2NUserId = Guid.NewGuid() };

            var promiseSrv = new N2NPromiseService(_repo.Object, _securSrv.Object);
            Assert.Throws(typeof(N2NSecurityException), delegate { promiseSrv.Delete(promise.Id); });
        }


        [Test]
        public void Should_Delete_Promise_For_Another_UserId_If_Admin()
        {
            var promise = new N2NPromise() { N2NUserId = Guid.NewGuid() };
            var promiseSrv = new N2NPromiseService(_repo.Object, _adminSecurSrv.Object);

            var editedPromise = promiseSrv.Delete(promise.Id);

            Assert.AreEqual(editedPromise.Id, promise.Id);

        }
    }
}
