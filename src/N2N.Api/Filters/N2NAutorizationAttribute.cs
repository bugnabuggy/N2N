using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Helpers;
using N2N.Api.Services;
using N2N.Core.Constants;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Api.Filters
{
    public class N2NAutorizationAttribute : ActionFilterAttribute
    {
        private readonly string _role;

        public N2NAutorizationAttribute(string role = N2NRoles.User)
        {
            this._role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Filters.Any(x => x is IAllowAnonymousFilter)){ goto end; }

            var authService = context.HttpContext.RequestServices.GetService<IAuthenticationService>();
            var authHeader = context.HttpContext.Request.Headers["Authorization"];
            var authResult = await authService.AuthenticateByAuthHeaderAsync(authHeader);

            if (!authResult.Success)
            {
                context.Result = new ObjectResult(authResult.Messages) { StatusCode = (int)HttpStatusCode.Unauthorized };
                return;
            }

            var roles = await authService.GetUserRolesAsync((authResult.Data as N2NUser).NickName);
            if (!roles.Contains(this._role))
            {
                context.Result = new ObjectResult(new[] { $"You are not in [{this._role}] role" }) { StatusCode = (int)HttpStatusCode.Forbidden };
                return;
            }

            var claims = ClaimsGenerator.GetClaims(roles, user: authResult.Data);
            var identity = new N2NIdentity(authResult.Data as N2NUser, claims);
            context.HttpContext.User = new ClaimsPrincipal(identity);

            end:
            if (next != null)
            {
                await base.OnActionExecutionAsync(context, next);
            }
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }


    }
}
