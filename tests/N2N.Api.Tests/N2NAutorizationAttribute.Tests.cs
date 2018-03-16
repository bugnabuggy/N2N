using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        
        private IN2NTokenService _tokenService;
        private ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void SetUp()
        {
            _serviceProvider = new DatabaseDiBootstrapperInMemory().GetServiceProviderWithSeedDB().Result;
            _tokenService = _serviceProvider.GetService<IN2NTokenService>();

            _filter = new N2NAutorizationAttribute();
        }

        [Test]
        public void Should_pass_all_if_action_has_IAllowAnonymous_attribute()
        {
            var principal = Thread.CurrentPrincipal;
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

            _filter.OnActionExecuting(ctx);

            Assert.IsNull(ctx.Result);
            Assert.AreEqual(Thread.CurrentPrincipal, principal);
        }

        [Test]
        public void Should_return_http_401_if_not_authorized_successfully()
        {
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

            _filter.OnActionExecuting(ctx);

            Assert.AreEqual((ctx.Result as ObjectResult).StatusCode, (int)HttpStatusCode.Unauthorized);
            Assert.AreEqual(((ctx.Result as ObjectResult).Value as IEnumerable<string>).FirstOrDefault(),
                "You do not have Authorization header");

            actionContext.HttpContext.Request.Headers.Add("Authorization", "Basic X");
            _filter.OnActionExecuting(ctx);

            Assert.AreEqual((ctx.Result as ObjectResult).StatusCode, (int)HttpStatusCode.Unauthorized);
            Assert.AreEqual(((ctx.Result as ObjectResult).Value as IEnumerable<string>).FirstOrDefault(),
                "Your Authorization header is corruped or do not use Bearer scheme");
        }

        [Test]
        public void Should_set_principal_and_roles_for_user_via_token()
        {
            var principal = Thread.CurrentPrincipal;
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

            actionContext.HttpContext.Request.Headers.Add("Authorization", "Bearer " + tokenString);

            _filter.OnActionExecuting(ctx);

            Assert.AreNotEqual(Thread.CurrentPrincipal, principal);
            Assert.IsTrue(Thread.CurrentPrincipal.IsInRole(N2NRoles.User));
            Assert.IsTrue(Thread.CurrentPrincipal.Identity.Name.Equals(user.NickName));
        }
    }
}
