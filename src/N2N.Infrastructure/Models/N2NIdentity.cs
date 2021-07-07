using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Infrastructure.Models
{
    public class N2NIdentity : ClaimsIdentity
    {
        public N2NUser N2NUser { get; }

        public N2NIdentity(N2NUser user, IEnumerable<Claim> claims, string authType = "N2N") : base(claims, authType)
        {
            this.N2NUser = user;
        }
    }
}
