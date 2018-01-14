using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using N2N.Api.Configuration;
using N2N.Core.Entities;
using N2N.Core.Models;
using N2N.Core.Services;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Api.Services
{
    public class N2NApiUserService
    {
        private IN2NUserService _userService;
        private UserManager<N2NIdentityUser> _userManager;

        public N2NApiUserService(IN2NUserService userService, UserManager<N2NIdentityUser> userManager)
        {
            this._userService = userService;
            this._userManager = userManager;
        }

        public async Task<OperationResult> CreateUserAsync(N2NUser user, string password)
        {
            // this all should be a transaction ↓

            OperationResult result= new OperationResult();


            if (!this._userService.IsNicknameExists(user))
            {
                user = this._userService.CheckOrRegenerateUserId(user);

                var identityResult = await this._userManager.CreateAsync(new N2NIdentityUser()
                {
                    UserName = user.NickName,
                    N2NUserId = user.Id
                }, password);

                //if operation fails, we should delete N2NUser
                if (identityResult.Succeeded)
                {
                    result = this._userService.CreateUser(user);
                }
            } else
            {
                result.Messages = new[] { $" User with nickname {user.NickName} already exists!" };
            }

            return result;
        }
    }
}
