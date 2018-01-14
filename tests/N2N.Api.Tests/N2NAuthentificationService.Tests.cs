using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using N2N.TestData.SupportClasses;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace N2N.Services.Tests
{
    [TestFixture()]
    class N2NAuthentificationServiceTests
    {
        private IRepository<N2NToken> _tokenRepo;
        private IRepository<N2NRefreshToken> _refreshTokenRepo;
        private IRepository<N2NUser> _userRepo;
        private UserManager<N2NIdentityUser> _userManager;

        private N2NDataContext _dbContext;
        [SetUp]
        public void Start()
        {
            _tokenRepo = new MockRepository<N2NToken>(N2N.TestData.TokenList.GetList());
            _refreshTokenRepo = new MockRepository<N2NRefreshToken>(N2N.TestData.RefreshTokenList.GetList());
            _userRepo = new MockRepository<N2NUser>(N2N.TestData.N2NUsersList.GetList());

            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<N2NDataContext>(options => options.UseInMemoryDatabase("Test"));

            services.AddIdentity<N2NIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<N2NDataContext>();
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = context });
            var serviceProvider = services.BuildServiceProvider();
            _dbContext = serviceProvider.GetRequiredService<N2NDataContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<N2NIdentityUser>>();
            
        }

        [Test]
        public void Should_get_user_name_from_token_claims()
        {
            var authSrv = new N2N.Api.Services.AuthenticationService(_userManager, _tokenRepo, _refreshTokenRepo, _userRepo);

            var userName = authSrv.GetUserName(N2N.TestData.AthorizationMocks.GetAuthorizationHeaderTokenString());

            Assert.AreEqual(userName, "An");
        }

        [Test]
        public void Should_validate_token_string_and_return_user_in_result_data()
        {
            var authSrv = new N2N.Api.Services.AuthenticationService(_userManager, _tokenRepo, _refreshTokenRepo, _userRepo);

            var validationResult =
                authSrv.GetUserByTokenString(N2N.TestData.AthorizationMocks.GetAuthorizationHeaderTokenString());

            Assert.IsTrue(validationResult.Success);
            Assert.AreEqual((validationResult.Data as N2NUser).NickName, "An");
        }

        [Test]
        public void Should_return_operation_result_do_not_have_Authorization_header()
        {
            var authSrv = new N2N.Api.Services.AuthenticationService(_userManager, _tokenRepo, _refreshTokenRepo, _userRepo);
            var authHeader = "";

            var result = authSrv.AuthenticateByToken(authHeader);

            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Messages.Count(), 1);
            Assert.AreEqual((result.Messages as List<string>)[0], "You do not have Authorization header");
        }

        [Test]
        public void Should_return_operation_result_invalid_token_in_Authorization_header()
        {
            var authSrv = new N2N.Api.Services.AuthenticationService(_userManager, _tokenRepo, _refreshTokenRepo, _userRepo);
            var authHeader = "aaddd nash aaaad nnaaash!!!!";

            var result = authSrv.AuthenticateByToken(authHeader);

            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Messages.Count(), 2);
            Assert.AreEqual((result.Messages as List<string>)[1], "User have not been found in token validation data");
        }

        [Test]
        public void Should_return_operation_result_success_and_set_current_principal_for_the_thread()
        {
            var authSrv = new N2N.Api.Services.AuthenticationService(_userManager, _tokenRepo, _refreshTokenRepo, _userRepo);
            var authHeader = N2N.TestData.AthorizationMocks.GetAuthorizationHeaderTokenString();
            var activeUserName = Thread.CurrentPrincipal.Identity.Name;

            var result = authSrv.AuthenticateByToken(authHeader);

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Messages.Count(), 0);
            Assert.AreEqual((result.Data as N2NUser).NickName, "An");
        }
    }
}
