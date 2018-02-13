using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.TestData.Helpers;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace N2N.Api.Tests
{
    [TestFixture()]
    public class TokenServiceTests
    {
        private IRepository<N2NToken> _tokenRepo;
        private IRepository<N2NRefreshToken> _refreshTokenRepo;
        private IConfiguration _configuration;


        [OneTimeSetUp]
        public async void TestSetUp()
        {
            var provider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            
            _tokenRepo = provider.GetService<IRepository<N2NToken>>();
            _refreshTokenRepo = provider.GetService<IRepository<N2NRefreshToken>>();

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Token:Lifetime"]).Returns("30");
            _configuration = configuration.Object;
        }

        [Test]
        public void Should_generate_N2NToken_string_and_add_token_to_database()
        {
            var tokenSrv = new TokenService(_tokenRepo, _refreshTokenRepo, _configuration);
            var user = TestData.N2NUsersList.GetList().ToArray()[0];
            DateTime expirationDate;

            var token = tokenSrv.GetN2NToken(user.Id, user.NickName, out expirationDate);

            Assert.IsTrue(_tokenRepo.Data.Any(t=>t.Id.Equals(Guid.Parse(token.Id))));
        }
    }
}
