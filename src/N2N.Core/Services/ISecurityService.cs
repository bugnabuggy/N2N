using System;
using System.Security.Principal;

namespace N2N.Core.Services
{
    public interface ISecurityService
    {
        bool HasAccess();
        bool HasAccess(IIdentity identity);

        Guid GetCurrentN2NUserId();
    }
}
