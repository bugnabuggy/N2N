using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Infrastructure.Models
{
    public class N2NIdentity : IIdentity
    {
        public N2NUser N2NUser { get; }

        public string Name { get; }
        public string AuthenticationType { get; }
        public bool IsAuthenticated { get; }

        public N2NIdentity(N2NUser user, bool isAuthenticated)
        {
            this.N2NUser = user;
            this.AuthenticationType = "N2N";
            this.Name = user.NickName;
            this.IsAuthenticated = isAuthenticated;
        }
    }
}
