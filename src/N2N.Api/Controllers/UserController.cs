using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;

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

        [HttpPost("/user/register")]
        public async Task<IActionResult> Register([FromBody] UserDataForm userData)
        {
          
            if (!userData.NickName.IsNullOrEmpty() && 
                !userData.Password.IsNullOrEmpty() && 
                !userData.Capcha.IsNullOrEmpty() )
            {
                N2NUser user = new N2NUser() { NickName = userData.NickName };
                var result = await this._apiUserService.CreateUserAsync(user, userData.Password);
                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
                return Ok(result.Messages);
            }
            else
            {
                return BadRequest("fill in all the fields");
            }

        }

    }
}
