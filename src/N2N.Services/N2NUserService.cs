using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Services
{
    public class N2NUserService : IN2NUserService
    {
        private IRepository<N2NUser> _userRepo;
        private ISecurityService _security;


        public N2NUser CheckOrRegenerateUserId(N2NUser user)
        {
            while (this._userRepo.Data.Any( x => x.Id == user.Id))
            {
                user.Id = Guid.NewGuid();
            }
            return user;
        }

        public N2NUserService(IRepository<N2NUser> userRepo, ISecurityService security)
        {
            this._userRepo = userRepo;
            this._security = security;
        }

        public bool IsNicknameExists(N2NUser user)
        {
            var nickNameExists = this._userRepo.Data.Any(x => x.NickName == user.NickName);

            return nickNameExists;
        }

        public OperationResult CreateUser(N2NUser user)
        {
            var result = new OperationResult();

            //TODO: add verification of user fields

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
