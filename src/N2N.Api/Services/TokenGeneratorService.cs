using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using N2N.Api.Configuration;

namespace N2N.Api.Services
{
    public class TokenGeneratorService : ITokenGenerator
    {
        public JwtSecurityToken GetToken(N2NTokenConfig config)
        {
            var token = new JwtSecurityToken(
                    issuer: config.Issuer,
                    audience: config.Audience,
                    claims: config.Claims,
                    expires: config.ValidTill,
                    notBefore: config.ValidFrom
                );

            return token;
        }
    }
}
