using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;
using N2N.Services.Users;

namespace N2N.Api.Services
{
    public class N2NApiUserService
    {
        private IN2NUserService _userservice;
        private UserManager<N2NIdentityUser> _userManager;

        public N2NApiUserService(IN2NUserService userService, UserManager<N2NIdentityUser> userManager)
        {
            this._userservice = userService;
            this._userManager = userManager;
        }

        public async Task<OperationResult> CreateUserAsync(N2NUser user, string password)
        {
            // this all should be a transaction ↓

            var result = this._userservice.CreateUser(user);
            var identityResult = await this._userManager.CreateAsync(new N2NIdentityUser()
            {
                N2NUserId = user.Id
            }, password);
            
            //if operation fails, we should delete N2NUser

            return result;
        }
    }
}
