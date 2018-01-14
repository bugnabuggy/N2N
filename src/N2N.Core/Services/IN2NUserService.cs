using N2N.Core.Entities;
using N2N.Core.Models;

namespace N2N.Core.Services
{
    public interface IN2NUserService
    {
        OperationResult CreateUser(N2NUser user);
        bool IsNicknameExists(string  nickname);
        N2NUser CheckOrRegenerateUserId(N2NUser user);
    }
}
