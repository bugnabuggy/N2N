using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using N2N.Api.Filters;
using N2N.Api.Services;
using N2N.Core.Constants;
using N2N.TestData.Helpers;
using NUnit.Framework;

namespace N2N.Api.Tests
{
    [TestFixture]
    class N2NAutorizationAttributeTests
    {
        private N2NAutorizationAttribute _filter;
        
        //private IN2NTokenService _tokenService;
        //private ServiceProvider _serviceProvider;


        [SetUp]
        public async Task SetUp()
        {
            //_serviceProvider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            //_tokenService = _serviceProvider.GetService<IN2NTokenService>();

            _filter = new N2NAutorizationAttribute();
        }


        [Test]
        public async Task Should_pass_all_if_action_has_IAllowAnonymous_attribute()
        {
            ServiceProvider _serviceProvider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            var filterMetadata = new List<IFilterMetadata>() {new AllowAnonymousFilter()};
            var actionContext = new ActionContext(
                new DefaultHttpContext() { RequestServices = _serviceProvider },
                new RouteData(),
                new ActionDescriptor()
            );

            var ctx = new ActionExecutingContext(
                actionContext, 
                filterMetadata, 
                new Dictionary<string, object>(),
                new object());

            var principal = ctx.HttpContext.User;

            await _filter.OnActionExecutionAsync(ctx, null);

            Assert.IsNull(ctx.Result);
            Assert.AreEqual(ctx.HttpContext.User, principal);
        }

        [Test]
        public async Task Should_return_http_401_if_not_authorized_successfully()
        {
            ServiceProvider _serviceProvider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            var filterMetadata = new List<IFilterMetadata>() {};
            var actionContext = new ActionContext(
                new DefaultHttpContext() { RequestServices = _serviceProvider },
                new RouteData(),
                new ActionDescriptor()
            );

            var ctx = new ActionExecutingContext(
                actionContext,
                filterMetadata,
                new Dictionary<string, object>(),
                new object());

            await _filter.OnActionExecutionAsync(ctx, null);

            Assert.AreEqual((ctx.Result as ObjectResult).StatusCode, (int)HttpStatusCode.Unauthorized);
            Assert.AreEqual(((ctx.Result as ObjectResult).Value as IEnumerable<string>).FirstOrDefault(),
                "You do not have Authorization header");

            actionContext.HttpContext.Request.Headers.Add("Authorization", "Basic X");
            await _filter.OnActionExecutionAsync(ctx, null);

            Assert.AreEqual((ctx.Result as ObjectResult).StatusCode, (int)HttpStatusCode.Unauthorized);
            Assert.AreEqual(((ctx.Result as ObjectResult).Value as IEnumerable<string>).FirstOrDefault(),
                "Your Authorization header is corruped or do not use Bearer scheme");
        }

        [Test]
        public async Task Should_set_principal_and_roles_for_user_via_token()
        {
            ServiceProvider _serviceProvider = await new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB();
            IN2NTokenService _tokenService = _serviceProvider.GetService<IN2NTokenService>();
            var filterMetadata = new List<IFilterMetadata>() {};
            var actionContext = new ActionContext(
                new DefaultHttpContext() { RequestServices = _serviceProvider },
                new RouteData(),
                new ActionDescriptor()
            );

            var ctx = new ActionExecutingContext(
                actionContext,
                filterMetadata,
                new Dictionary<string, object>(),
                new object());

            var user = TestData.N2NUsersList.GetList().FirstOrDefault();
            var tokenString = TestData.TokenList.GetTokenString(user, _tokenService);

            var principal = _serviceProvider.GetService<IPrincipal>();
            Assert.IsFalse(principal.IsInRole(N2NRoles.Admin));
            Assert.IsFalse(principal.IsInRole(N2NRoles.User));

            actionContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + tokenString);

            await _filter.OnActionExecutionAsync(ctx, null);

            Assert.IsTrue(ctx.HttpContext.User.IsInRole(N2NRoles.User));
            Assert.AreEqual(ctx.HttpContext.User.Identity.Name, user.NickName);

        }
    }
}
