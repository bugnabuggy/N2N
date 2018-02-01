using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Filters;
using N2N.Core.Constants;
using N2N.Core.Entities;
using N2N.Core.Models;
using N2N.Core.Services;
using N2N.Infrastructure.Models;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IN2NUserService _userService;
        private readonly IUsersStatisticsService _usersStatistics;

        public UsersController(IN2NUserService userService, IUsersStatisticsService usersStatistics)
        {
            this._userService = userService;
            this._usersStatistics = usersStatistics;
        }

        [HttpGet("/users")]
        [N2NAutorization(N2NRoles.Admin)]
        public ApiResult<N2NUser> Get()
        {
            //TODO: implement filters 
            var users = _userService.GetUsers().ToList();
            return new ApiResult<N2NUser>()
            {
                Count = users.Count,
                Data = users,
                PageSize = 0
            };
        }

        [HttpGet("/users/statistics")]
        [N2NAutorization(N2NRoles.Admin)]
        public ApiResult<UserStatistics> GetUsesrStatistics()
        {
            //TODO: implement filters 
            var statistics = _usersStatistics.GetUsersStatistics(filter: null, orderBy: null).ToList();
            return new ApiResult<UserStatistics>()
            {
                Count = statistics.Count,
                Data = statistics,
                PageSize = 0
            };
        }

        [HttpGet("/users/{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        [HttpPost("/users")]
        public void Post([FromBody]string value)
        {
        }
        
        
        [HttpPut("/users/{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("/users/{id}")]
        public void Delete(int id)
        {
        }
    }
}
