using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Api.Filters
{
    public class N2NAutorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context.HttpContext.RequestServices.GetService<IAuthenticationService>();
            var authHeader = context.HttpContext.Request.Headers["Authorization"];
            
            var authResult = authService.AuthenticateByToken(authHeader);

            if (!authResult.Success)
            {
                context.Result = new ObjectResult(authResult.Messages){StatusCode = (int)HttpStatusCode.Unauthorized};
            }

            //set user identity for the thread
            Thread.CurrentPrincipal = new GenericPrincipal(new N2NIdentity(authResult.Data as N2NUser, isAuthenticated: true), new string[] { });
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
