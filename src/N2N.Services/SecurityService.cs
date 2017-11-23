using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace N2N.Services
{
    public class SecurityService : ISecurityService
    {
        public bool HasAccess()
        {
            var principal = System.Threading.Thread.CurrentPrincipal;

            return true;
        }

        public bool HasAccess(IIdentity identity)
        {
            throw new NotImplementedException();
        }
    }
}
