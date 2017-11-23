using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Services
{
    public interface ISecurityService
    {
        bool HasAccess();
        bool HasAccess(IIdentity identity);
    }
}
