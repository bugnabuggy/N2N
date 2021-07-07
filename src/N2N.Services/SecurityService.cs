using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using N2N.Core.Enums;
using N2N.Core.Interfaces;
using N2N.Core.Services;
using N2N.Infrastructure.Exceptions;
using N2N.Infrastructure.Models;

namespace N2N.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IPrincipal _principal;

        public SecurityService(IPrincipal principal)
        {
            this._principal = principal;
        }

        public Guid GetCurrentN2NUserId()
        {
            if (_principal.Identity is N2NIdentity)
            {
                return (_principal.Identity as N2NIdentity).N2NUser.Id;
            }
            else
            {
                throw new N2NSecurityException("There is no current authenticated user.");
            }
        }

        public bool HasAccess()
        {
            return _principal.IsInRole("Admin") || System.Threading.Thread.CurrentPrincipal.IsInRole("Admin");
        }

        public bool HasAccess(IIdentity identity)
        {
            throw new NotImplementedException();
        }


        public bool HasAccess(IOwned property)
        {
            return property.N2NUserId == (_principal.Identity as N2NIdentity).N2NUser.Id;
        }

        public bool HasAccess(IOwned property, N2NIdentity identity)
        {
            return property.N2NUserId == identity.N2NUser.Id;
        }

        public bool HasAccess(N2NActions action, N2NIdentity identity)
        {
            throw new NotImplementedException();
        }
    }
}
