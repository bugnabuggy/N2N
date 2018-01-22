﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Core.Models;
using N2N.Core.Services;
using N2N.Data.Repositories;

namespace N2N.Services
{
    public class N2NUserService : IN2NUserService
    {
        private IRepository<N2NUser> _userRepo;
        private ISecurityService _security;


        public N2NUser CheckOrRegenerateUserId(N2NUser user)
        {
            if(user.Id == Guid.Empty) { user.Id = Guid.NewGuid(); }
            while (this._userRepo.Data.Any( x => x.Id == user.Id))
            {
                user.Id = Guid.NewGuid();
            }
            return user;
        }

        public IEnumerable<N2NUser> GetUsers(Expression<Func<N2NUser, bool>> filter = null,
            Func<IQueryable<N2NUser>, IOrderedQueryable<N2NUser>> orderBy = null)
        {

            IQueryable<N2NUser> query = _userRepo.Data;

            if (filter != null)
            {
                query = _userRepo.Data.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public N2NUserService(IRepository<N2NUser> userRepo, ISecurityService security)
        {
            this._userRepo = userRepo;
            this._security = security;
        }

        public bool IsNicknameExists(string nickname)
        {
            var nickNameExists = this._userRepo.Data.Any(x => x.NickName == nickname);

            return nickNameExists;
        }

        public OperationResult CreateUser(N2NUser user)
        {
            var result = new OperationResult();

            //TODO: add verification of user fields

            if (this._security.HasAccess())
            {
                user.Registration = DateTime.UtcNow;

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
