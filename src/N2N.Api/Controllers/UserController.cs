﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Filters;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using Newtonsoft.Json;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    public class UserController : Controller
    {
        private N2NApiUserService _apiUserService;
        private IAuthentificationService _authentificationService;


        public UserController(N2NApiUserService apiUserService, IAuthentificationService authentificationService)
        {
            this._authentificationService = authentificationService;
            this._apiUserService = apiUserService;
        }

        //[N2NAutorizationFilter]
        [HttpGet("/user")]
        public async Task<JsonResult> СheckUser()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader!="Bearer")
            {
                string welcome_message = "Welcome " + _authentificationService.GetNameUser(authHeader.ToString());
                return Json(welcome_message);
            }
            return Json("You not authentification");
        }


        [HttpPost("/user/logIn")]
        public async Task<IActionResult> LogIn([FromBody] UserRegistrationFormDTO userRegistration)
        {

            if (!userRegistration.NickName.IsNullOrEmpty() &&
                !userRegistration.Password.IsNullOrEmpty() &&
                !userRegistration.Capcha.IsNullOrEmpty())
            {
                var response =
                    await _authentificationService.Authentification(userRegistration.NickName,
                        userRegistration.Password);

                if (response.ToString() == new { }.ToString())
                {
                    return BadRequest("Invalid username or password.");

                }
                return Ok(response);
            }
            else
            {
                return BadRequest("fill in all the fields");
            }
        }

        [HttpPost("/user/register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationFormDTO userRegistration)
        {
          
            if (!userRegistration.NickName.IsNullOrEmpty() && 
                !userRegistration.Password.IsNullOrEmpty() && 
                !userRegistration.Capcha.IsNullOrEmpty() )
            {
                N2NUser user = new N2NUser()
                {
                    Id = Guid.NewGuid(),
                    NickName = userRegistration.NickName
                };

                var result = await this._apiUserService.CreateUserAsync(user, userRegistration.Password);

                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }

                var response =await _authentificationService.Authentification(userRegistration.NickName, userRegistration.Password);

                if (response.ToString() == new { }.ToString())
                {
                    return BadRequest("Invalid username or password.");
                   
                }
                
                return Ok(response);
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
