using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;

namespace N2N.Services.Users
{
    public interface IN2NUserService
    {
        OperationResult CreateUser(N2NUser user);
    }
}
