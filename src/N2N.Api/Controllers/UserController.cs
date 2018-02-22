using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Isam.Esent.Interop;
using N2N.Api.Filters;
using N2N.Api.Services;
using N2N.Core.Constants;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;
using Newtonsoft.Json;

// TODO: refactor this ↓
namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    public class UserController : Controller
    {
        private N2NApiUserService _apiUserService;
        private IAuthenticationService _authentificationService;


        public UserController(N2NApiUserService apiUserService, IAuthenticationService authentificationService)
        {
            this._authentificationService = authentificationService;
            this._apiUserService = apiUserService;
        }

        [HttpGet("/user/IsAuthorization")]
        public async Task<bool> IsAuthorization()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader != "Bearer undefined")
            {
                return true;
            }
            return false;
        }

        //[N2NAutorizationFilter]
        [HttpGet("/user")]
        public JsonResult СheckUser()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader!= "Bearer undefined")
            {
                string welcome_message = "Welcome " + _authentificationService.GetUserName(authHeader.ToString());
                return Json(welcome_message);
            }
            return Json("You not authentification");
        }


        [HttpPost("/user/logIn")]
        public async Task<IActionResult> LogIn([FromBody] UserLoginDTO loginForm)
        {

            if (!string.IsNullOrEmpty(loginForm.NickName) &&
                !string.IsNullOrEmpty(loginForm.Password) &&
                !string.IsNullOrEmpty(loginForm.Captcha))
            {
                var response =
                    await _authentificationService.AuthenticateUserAsync(loginForm.NickName,
                        loginForm.Password);

                if (response.Data.ToString() == new { }.ToString())
                {
                    return BadRequest("Invalid username or password.");

                }
                return Ok(response.Data);
            }
            else
            {
                return BadRequest("fill in all the fields");
            }
        }

        [HttpPost("/user/register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationFormDTO userRegistration)
        {
          
            if (!string.IsNullOrEmpty(userRegistration.NickName) && 
                !string.IsNullOrEmpty(userRegistration.Password) && 
                !string.IsNullOrEmpty(userRegistration.Captcha) )
            {
                N2NUser user = new N2NUser()
                {
                    Id = Guid.NewGuid(),
                    NickName = userRegistration.NickName
                };

                // because we have service permission checks 
                System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("N2N User Registration Service"), new [] { N2NRoles.Admin } );

                var result = await this._apiUserService.CreateUserAsync(user, userRegistration.Password);

                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }

                var response = await _authentificationService.AuthenticateUserAsync(userRegistration.NickName, userRegistration.Password);

                if (response.Data.ToString() == new { }.ToString())
                {
                    return BadRequest("Invalid username or password.");
                   
                }
                
                return Ok(response.Data);
            }
            else
            {
                return BadRequest("fill in all the fields");
            }

        }

        [HttpDelete("/user/LogOut")]
        public void LogOut()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            this._authentificationService.DeleteToken(authHeader.ToString());
        }

    }
}
