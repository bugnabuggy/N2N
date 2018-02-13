﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using N2N.Data.Repositories;
using N2N.TestData.Helpers;

namespace N2N.Api.Tests
{
    [TestFixture()]
    class TonyAuthServiceTests
    {
        private UserManager<N2NIdentityUser> _userManager;
        private IRepository<N2NUser> _userRepo;
        private IConfiguration _configuration;

        [OneTimeSetUp]
        public async void TestSetUp()
        {
            var provider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            _userManager = provider.GetService<UserManager<N2NIdentityUser>>();
            _userRepo = provider.GetService<IRepository<N2NUser>>();

            var tokenGenerator = new TokenService();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Token:Lifetime"]).Returns("30");
            _configuration = configuration.Object;
        }

        [OneTimeTearDown]
        public void TestTearDown()
        {

        }

        [Test]
        public async Task Should_return_auth_result_object()
        {
            var authSrv = new TonyAuthService(userManager, userRepo, tokenRepo, refreshTokenRepo, tokenGenerator, configuration.Object);

            var authResult =
                await authSrv.AuthenticateUserAsync(TestData.N2NUsersList.GetList().ToArray()[0].NickName, HardCoddedConfig.DefaultPassword);

            Assert.IsTrue(authResult.Data != null);
            Assert.IsTrue(authResult.Success);


        }

        [Test]
        public async Task Should_get_token_object()
        {
            var provider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            var userManager = provider.GetService<UserManager<N2NIdentityUser>>();
            var userRepo = provider.GetService<IRepository<N2NUser>>();
            var tokenRepo = provider.GetService<IRepository<N2NToken>>();
            var refreshTokenRepo = provider.GetService<IRepository<N2NRefreshToken>>();
            var tokenGenerator = new TokenService();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Token:Lifetime"]).Returns("30");

            var authSrv = new TonyAuthService(userManager, userRepo, tokenRepo, refreshTokenRepo, tokenGenerator, configuration.Object);

            var authResult =
                await authSrv.AuthenticateUserAsync(TestData.N2NUsersList.GetList().ToArray()[0].NickName, HardCoddedConfig.DefaultPassword);

            Assert.IsTrue(authResult.Data != null);
            Assert.IsTrue(authResult.Success);


        }
    }
}
