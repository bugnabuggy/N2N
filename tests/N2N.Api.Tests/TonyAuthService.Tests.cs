﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models.DTO;
using N2N.TestData.Helpers;

namespace N2N.Api.Tests
{
    [TestFixture()]
    class TonyAuthServiceTests
    {
        private UserManager<N2NIdentityUser> _userManager;
        private IRepository<N2NUser> _userRepo;  
        private IRepository<N2NToken> _tokenRepo; 
        private IRepository<N2NRefreshToken> _refreshTokenRepo;
        private IConfiguration _configuration;
        private IN2NTokenService _tokenService;


        [OneTimeSetUp]
        public async Task TestSetUp()
        {
            var provider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            _userManager = provider.GetService<UserManager<N2NIdentityUser>>();
            _userRepo = provider.GetService<IRepository<N2NUser>>();
            _tokenRepo = provider.GetService<IRepository<N2NToken>>();
            _refreshTokenRepo = provider.GetService<IRepository<N2NRefreshToken>>();

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Token:Lifetime"]).Returns("30");
            _configuration = configuration.Object;

            _tokenService = new TokenService(_tokenRepo, _refreshTokenRepo, _configuration);
        }

        [OneTimeTearDown]
        public void TestTearDown()
        {

        }

        [Test]
        public async Task Should_return_auth_result_object()
        {
            var authSrv = new TonyAuthService(_userManager, _userRepo, _tokenService, _configuration);
            var tokenCount = _tokenRepo.Data.Count();
            var refreshTokenCount = _refreshTokenRepo.Data.Count();

            var authResult =
                await authSrv.AuthenticateUserAsync(TestData.N2NUsersList.GetList().ToArray()[0].NickName, HardCoddedConfig.DefaultPassword);

            Assert.IsTrue(authResult.Data != null);
            Assert.IsTrue(authResult.Success);
            var tokenResultDTO = authResult.Data as AuthenticationResponseDTO;

            Assert.IsTrue(!tokenResultDTO.access_token.IsNullOrEmpty());
            Assert.IsTrue(!tokenResultDTO.refresh_token.IsNullOrEmpty());
            Assert.IsTrue(tokenResultDTO.expiration_date > DateTime.UtcNow);

            Assert.IsTrue(_tokenRepo.Data.Count() == tokenCount + 1);
            Assert.IsTrue(_refreshTokenRepo.Data.Count() == refreshTokenCount + 1);
        }
        
        [Test]
        public async Task Should_return_failde_result_if_user_not_exists()
        {
            var authSrv = new TonyAuthService(_userManager, _userRepo, _tokenService, _configuration);

            var authResult =
                await authSrv.AuthenticateUserAsync("nouser", HardCoddedConfig.DefaultPassword);

            Assert.IsFalse(authResult.Success);
            Assert.AreEqual(authResult.Messages.FirstOrDefault(), "Password is wrong or user not exists");
        }

        [Test]
        public async Task Should_return_failed_result_if_password_is_wrong()
        {
            var authSrv = new TonyAuthService(_userManager, _userRepo, _tokenService, _configuration);

            var authResult =
                await authSrv.AuthenticateUserAsync(TestData.N2NUsersList.GetList().ToArray()[0].NickName, "nopassword");

            Assert.IsFalse(authResult.Success);
            Assert.AreEqual(authResult.Messages.FirstOrDefault(), "Password is wrong or user not exists");
        }

        [Test]
        public async Task Should_return_failed_result_if_security_user_exists_but_n2nuser_not()
        {
            var authSrv = new TonyAuthService(_userManager, _userRepo, _tokenService, _configuration);

            var authResult =
                await authSrv.AuthenticateUserAsync(TestData.N2NUsersList.GetN2NIdentityUser().UserName, HardCoddedConfig.DefaultPassword);

            Assert.IsFalse(authResult.Success);
            Assert.AreEqual(authResult.Messages.FirstOrDefault(), "Identity user exists but N2NUser not found in the database");
        }


        [Test]
        public void Should_get_token_object()
        {
            var authSrv = new TonyAuthService(_userManager, _userRepo, _tokenService, _configuration);

            var authResult =
                 authSrv.AuthenticateByToken("");
;
            Assert.IsTrue(authResult.Data != null);
            Assert.IsTrue(authResult.Success);


        }
    }
}