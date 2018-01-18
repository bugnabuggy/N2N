using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using N2N.Core.Interfaces;
using N2N.Core.Services;
using N2N.Infrastructure.Exceptions;
using N2N.Infrastructure.Models;

namespace N2N.Services
{
    public class SecurityService : ISecurityService
    {
        public Guid GetCurrentN2NUserId()
        {
            if (Thread.CurrentPrincipal.Identity is N2NIdentity)
            {
                return (Thread.CurrentPrincipal.Identity as N2NIdentity).N2NUser.Id;
            }
            else
            {
                throw new N2NSecurityException("There is no current authenticated user.");
            }
        }

        public bool HasAccess()
        {
            var principal = System.Threading.Thread.CurrentPrincipal;
            return principal.IsInRole("Admin");
        }

        public bool HasAccess(IIdentity identity)
        {
            throw new NotImplementedException();
        }


        public bool HasAccess(IOwned property)
        {
            var principal = System.Threading.Thread.CurrentPrincipal;
            return property.N2NUserId == (principal.Identity as N2NIdentity).N2NUser.Id;
        }

        public bool HasAccess(IOwned property, N2NIdentity identity)
        {
            return property.N2NUserId == identity.N2NUser.Id;
        }
    }
}
