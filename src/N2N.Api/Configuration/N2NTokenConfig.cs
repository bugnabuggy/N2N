using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace N2N.Api.Configuration
{
    public class N2NTokenConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime ValidTill { get; set; }
        public DateTime ValidFrom { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
