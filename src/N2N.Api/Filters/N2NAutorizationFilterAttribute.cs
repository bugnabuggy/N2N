using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Services;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Api.Filters
{
    public class N2NAutorizationFilterAttribute : ActionFilterAttribute
    {
        
  

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            var service = context.HttpContext.RequestServices.GetService<IAuthentificationService>();
            

            ObjectResult result = new ObjectResult("");

            var authHeader = context.HttpContext.Request.Headers["Authorization"];
            

            if (authHeader.IsNullOrEmpty())
            {
                result = new ObjectResult("you do not have Authorization header");
                result.StatusCode = 401;
            }
            else
            {
                
                var tokenValidationResult = service.ValidateTokenString(authHeader.ToString());

                if (!tokenValidationResult.Success)
                {
                    result = new ObjectResult(tokenValidationResult.Messages);
                    result.StatusCode = 401;
                }
            }

            context.Result = result;
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
