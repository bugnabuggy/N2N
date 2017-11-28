using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;

namespace N2N.Services
{
    public interface IN2NUserService
    {
        OperationResult CreateUser(N2NUser user);
        bool IsNicknameExists(N2NUser user);
        N2NUser CheckOrRegenerateUserId(N2NUser user);
    }
}
