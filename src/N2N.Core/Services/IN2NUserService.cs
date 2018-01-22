using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using N2N.Core.Entities;
using N2N.Core.Models;

namespace N2N.Core.Services
{
    public interface IN2NUserService
    {
        OperationResult CreateUser(N2NUser user);
        bool IsNicknameExists(string  nickname);
        N2NUser CheckOrRegenerateUserId(N2NUser user);


        IEnumerable<N2NUser> GetUsers(Expression<Func<N2NUser, bool>> filter = null,
            Func<IQueryable<N2NUser>, IOrderedQueryable<N2NUser>> orderBy = null);
    }
}
