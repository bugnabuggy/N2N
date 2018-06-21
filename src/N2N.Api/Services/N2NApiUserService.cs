using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using N2N.Api.Configuration;
using N2N.Core.Constants;
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
        private RoleManager<IdentityRole> _roleManager;

        public N2NApiUserService(   IN2NUserService userService, 
                                    UserManager<N2NIdentityUser> userManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            this._userService = userService;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<OperationResult> CreateUserAsync(N2NUser user, string password, string[] roles = null)
        {
            // this all should be a transaction ↓

            OperationResult result= new OperationResult();
            roles = roles ?? new[] {N2NRoles.User};

            if (!this._userService.IsNicknameExists(user.NickName))
            {
                user = this._userService.CheckOrRegenerateUserId(user);

                var identityUser = new N2NIdentityUser()
                {
                    UserName = user.NickName,
                    N2NUserId = user.Id
                };

                var identityResult = await this._userManager.CreateAsync(identityUser, password);

                //if operation fails, we should delete N2NUser
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(identityUser, roles);
                    result = this._userService.CreateUser(user);

                    if (!result.Success)
                    {
                        await _userManager.RemoveFromRolesAsync(identityUser, roles);
                        await _userManager.DeleteAsync(identityUser);
                    }
                }
            } else
            {
                result.Messages = new[] { $" User with nickname {user.NickName} already exists!" };
            }

            return result;
        }

        public async Task<bool> UserExistsAndConsistentAsync(string nickname)
        {
            var identityUserExists = await _userManager.FindByNameAsync(nickname) != null;
            var n2nUserExists = _userService.IsNicknameExists(nickname);

            return identityUserExists && n2nUserExists;
        }
    }
}
