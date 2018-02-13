using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using N2N.Api.Configuration;

namespace N2N.Api.Services
{
    public interface IN2NTokenService
    {
        JwtSecurityToken GetToken(N2NTokenConfig config);
        JwtSecurityToken GetN2NToken(Guid userId, string nickname, out DateTime expirationDate);
        JwtSecurityToken GetN2NRefreshToken(Guid userId, string nickname, out DateTime expirationDate);
    }
}
