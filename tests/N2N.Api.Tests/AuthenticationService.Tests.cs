using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using N2N.Api.Filters;
using N2N.Core.Interfaces;
using N2N.Infrastructure.Repositories;
using N2N.Infrastructure.Models.DTO;
using N2N.TestData.Helpers;

namespace N2N.Api.Tests
{
    [TestFixture()]
    class AuthenticationServiceTests
    {
        private UserManager<N2NIdentityUser> _userManager;
        private IRepository<N2NUser> _userRepo;  
        private IRepository<N2NToken> _tokenRepo; 
        private IRepository<N2NRefreshToken> _refreshTokenRepo;
        private IConfiguration _configuration;
        private IN2NTokenService _tokenService;
        private IAuthenticationService _authSrv;

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
            _authSrv = new AuthenticationService(_userManager, _userRepo, _tokenRepo, _refreshTokenRepo, _tokenService, null);
        }

        [OneTimeTearDown]
        public void TestTearDown()
        {

        }

        [Test]
        public async Task Should_return_auth_result_object()
        {
            var tokenCount = _tokenRepo.Data.Count();
            var refreshTokenCount = _refreshTokenRepo.Data.Count();

            var authResult =
                await _authSrv.LoginUserAsync(TestData.N2NUsersList.GetList().ToArray()[0].NickName, HardCoddedConfig.DefaultPassword);

            Assert.IsTrue(authResult.Data != null);
            Assert.IsTrue(authResult.Success);
            var tokenResultDTO = authResult.Data as LoginResponseDTO;

            Assert.IsTrue(!tokenResultDTO.access_token.IsNullOrEmpty());
            Assert.IsTrue(!tokenResultDTO.refresh_token.IsNullOrEmpty());
            Assert.IsTrue(tokenResultDTO.expiration_date > DateTime.UtcNow);

            Assert.IsTrue(_tokenRepo.Data.Count() == tokenCount + 1);
            Assert.IsTrue(_refreshTokenRepo.Data.Count() == refreshTokenCount + 1);
        }
        
        [Test]
        public async Task Should_return_failde_result_if_user_not_exists()
        {
            var authResult =
                await _authSrv.LoginUserAsync("nouser", HardCoddedConfig.DefaultPassword);

            Assert.IsFalse(authResult.Success);
            Assert.AreEqual(authResult.Messages.FirstOrDefault(), "Password is wrong or user not exists");
        }

        [Test]
        public async Task Should_return_failed_result_if_password_is_wrong()
        {
            var authResult =
                await _authSrv.LoginUserAsync(TestData.N2NUsersList.GetList().ToArray()[0].NickName, "nopassword");

            Assert.IsFalse(authResult.Success);
            Assert.AreEqual(authResult.Messages.FirstOrDefault(), "Password is wrong or user not exists");
        }

        [Test]
        public async Task Should_return_failed_result_if_security_user_exists_but_n2nuser_not()
        {
            var authResult =
                await _authSrv.LoginUserAsync(TestData.N2NUsersList.GetN2NIdentityUser().UserName, HardCoddedConfig.DefaultPassword);

            Assert.IsFalse(authResult.Success);
            Assert.AreEqual(authResult.Messages.FirstOrDefault(), "Identity user exists but N2NUser not found in the database");
        }


        [Test]
        public async Task Should_fail_auhtnetication_by_Authorization_header_if_it_is_empty_or_not_Bearer_type()
        {
            var result = await _authSrv.AuthenticateByAuthHeaderAsync("");

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.AreEqual(result.Messages[0], "You do not have Authorization header");

            result = await _authSrv.AuthenticateByAuthHeaderAsync("hjkhkjsdhfjkshhjkhj");

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 2);
            Assert.AreEqual(result.Messages[0], "Your Authorization header is corruped or do not use Bearer scheme");

            result = await _authSrv.AuthenticateByAuthHeaderAsync("Basic hjkhkjsdhfjkshhjkhj");

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.AreEqual(result.Messages[0], "Your Authorization header is corruped or do not use Bearer scheme");
        }

        [Test]
        public async Task Should_authenticate_by_Authorization_header()
        {
            var user = TestData.N2NUsersList.GetList().FirstOrDefault();
            var tokenString = TestData.TokenList.GetTokenString(user, _tokenService);
            var authHeader = "Bearer " + tokenString;

            var result = await _authSrv.AuthenticateByAuthHeaderAsync(authHeader);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public async Task Should_authenticate_by_token_string()
        {
            var user = TestData.N2NUsersList.GetList().FirstOrDefault();
            var tokenString = TestData.TokenList.GetTokenString(user, _tokenService);

            var result = await _authSrv.AuthenticateByTokenStringAsync(tokenString);

            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf<N2NUser>(result.Data);
        }

        [Test]
        public async Task Should_fail_if_token_expired_or_not_start_effective()
        {
            var oldTokenString = TestData.AthorizationMocks.GetTokenString();

            var result = await _authSrv.AuthenticateByTokenStringAsync(oldTokenString);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public async Task Should_refresh_access_token()
        {
            var refreshToken =  _authSrv.LoginUserAsync(
                TestData.N2NUsersList.GetList().ToArray()[0].NickName,
                TestData.Helpers.HardCoddedConfig.DefaultPassword)
                    .Result
                    .Data
                    .refresh_token;

            var result = await _authSrv.RefreshAccessTokenAsync(refreshToken);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task Should_fail_when_token_string_is_wrong()
        {
            var refreshToken = ( await _authSrv.LoginUserAsync(
                    TestData.N2NUsersList.GetList().ToArray()[0].NickName,
                    TestData.Helpers.HardCoddedConfig.DefaultPassword))
                .Data
                .access_token;

            var result = await _authSrv.RefreshAccessTokenAsync(refreshToken);

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task Should_fail_when_refresh_token_expired()
        {
            var refreshToken = TestData.AthorizationMocks.GetExpiredRefreshToken(_refreshTokenRepo, _tokenService);

            var result = await _authSrv.RefreshAccessTokenAsync(refreshToken);

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task Should_delete_tokens()
        {
            var loginResult = await _authSrv.LoginUserAsync(
                TestData.N2NUsersList.GetList().ToArray()[0].NickName,
                TestData.Helpers.HardCoddedConfig.DefaultPassword
                );

            // access token
            var authResult = await _authSrv.AuthenticateByTokenStringAsync(loginResult.Data.access_token);
            Assert.IsTrue(authResult.Success);

            var delTokenResult = await _authSrv.DeleteTokenAsync<N2NToken>(loginResult.Data.access_token, _tokenRepo);
            Assert.IsTrue(delTokenResult.Success);

            authResult = await _authSrv.AuthenticateByTokenStringAsync(loginResult.Data.access_token);
            Assert.IsFalse(authResult.Success);

            // refresh token
         var refreshResult = await _authSrv.RefreshAccessTokenAsync(loginResult.Data.refresh_token);
            Assert.IsTrue(refreshResult.Success);

            var delRefreshResult = await _authSrv.DeleteTokenAsync<N2NRefreshToken>(loginResult.Data.refresh_token, _refreshTokenRepo);
            Assert.IsTrue(delRefreshResult.Success);

            refreshResult = await _authSrv.RefreshAccessTokenAsync(loginResult.Data.refresh_token);
            Assert.IsFalse(refreshResult.Success);
        }
    }
}
