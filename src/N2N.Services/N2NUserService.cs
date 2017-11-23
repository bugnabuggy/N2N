using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;
using N2N.Services.Users;

namespace N2N.Services
{
    public class N2NUserService : IN2NUserService
    {
        private IRepository<N2NUser> _userRepo;
        private ISecurityService _security;


        public N2NUserService(IRepository<N2NUser> userRepo, ISecurityService security)
        {
            this._userRepo = userRepo;
            this._security = security;
        }

        public OperationResult CreateUser(N2NUser user)
        {
            var result = new OperationResult()
            {
                Success = false
            };

            // add verification of user fields

            if (this._security.HasAccess())
            {
                //add user here
                _userRepo.Add(user);

                result.Data = user;
                result.Success = true;
                result.Messages = new[] { $"User was created with Id = ${user.Id}" };
            }
            else
            {
                result.Messages = new []{ "You dont have permissions to create user" };
            }

            return result;
        }
    }
}
