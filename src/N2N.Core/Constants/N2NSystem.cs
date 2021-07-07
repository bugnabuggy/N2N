using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Constants
{
    public class N2NSystem
    {
        public static GenericPrincipal GetN2NSystemPrincipal()
        {
            return new GenericPrincipal(new GenericIdentity("N2N system initialization"), new[] {N2NRoles.Admin});
        }
    }
}
