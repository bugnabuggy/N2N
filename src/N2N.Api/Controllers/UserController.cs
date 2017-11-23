using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using N2N.Services.Users;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    public class UserController : Controller
    {
        private N2NApiUserService _apiUserService;

        public UserController(N2NApiUserService apiUserService)
        {
            this._apiUserService = apiUserService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this._apiUserService.CreateUserAsync(new N2NUser()
                {
                    NickName = "Test",
                    Email = "test@test.com"
                }, 
                "Password"
            );

            if (!result.Success)
            {
                return  BadRequest( "Baaad");
            }

            return Ok();
        }
        
    }
}
