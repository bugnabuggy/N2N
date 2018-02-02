using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
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

            if(context.Filters.Any(x=>x is IAllowAnonymousFilter)) { return; }

            var authService = context.HttpContext.RequestServices.GetService<IAuthenticationService>();
            var authHeader = context.HttpContext.Request.Headers["Authorization"];
            
            var authResult = authService.AuthenticateByToken(authHeader);

            if (!authResult.Success)
            {
                context.Result = new ObjectResult(authResult.Messages){StatusCode = (int)HttpStatusCode.Unauthorized};
                return;
            }

            var roles = authService.GetUserRolesAsync((authResult.Data as N2NUser).NickName).Result;
            if (!roles.Contains(this._role))
            {
                context.Result = new ObjectResult( new []{$"You are not in [{this._role}] role"}) { StatusCode = (int)HttpStatusCode.Forbidden };
                return;
            }

            //set user identity for the thread
            Thread.CurrentPrincipal = new GenericPrincipal(new N2NIdentity(authResult.Data as N2NUser, isAuthenticated: true), roles: roles.ToArray() );
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            return base.OnActionExecutionAsync(context, next);
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
