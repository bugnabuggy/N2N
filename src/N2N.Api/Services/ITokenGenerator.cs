using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using N2N.Api.Configuration;

namespace N2N.Api.Services
{
    public interface ITokenGenerator
    {
        JwtSecurityToken GetToken(N2NTokenConfig config);
    }
}
