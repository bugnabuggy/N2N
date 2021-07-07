using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Api.Helpers
{
    public class ClaimsGenerator
    {
        public static IEnumerable<Claim> GetClaims(IEnumerable<string> roles, N2NUser user)
        {
            var claims = new List<Claim>();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(ClaimTypes.Name, user.NickName));

            return claims;
        }
    }
}
