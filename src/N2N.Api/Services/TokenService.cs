using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using N2N.Api.Configuration;
using N2N.Core.Entities;
using N2N.Data.Repositories;

namespace N2N.Api.Services
{
    public class TokenService : IN2NTokenService
    {
        private IRepository<N2NToken> _tokenRepo;
        private IRepository<N2NRefreshToken> _refreshTokenRepo;
        private IConfiguration _configuration;

        public TokenService(
            IRepository<N2NToken> tokenRepo,
            IRepository<N2NRefreshToken> refreshTokenRepo,
            IConfiguration configuration)
        {
            this._tokenRepo = tokenRepo;
            this._refreshTokenRepo = refreshTokenRepo;
            this._configuration = configuration;
        }

        public JwtSecurityToken GetN2NToken(Guid userId, string nickname, Guid refreshTokenId, out DateTime expirationDate)
        {
            var lifetime = 30;
            lifetime = int.TryParse(_configuration["Token:Lifetime"], out lifetime) ? lifetime : 30;

            expirationDate = DateTime.UtcNow.AddMinutes(lifetime);

            var n2nTokenRecord = new N2NToken()
            {
                Id = Guid.NewGuid(),
                N2NUserId = userId,
                RefreshTokenId = refreshTokenId,
                TokenExpirationDate = expirationDate
            };
            _tokenRepo.Add(n2nTokenRecord);
            var tokenClaims = GetClaims(nickname, n2nTokenRecord.Id);
            var tokenConfig = new N2NTokenConfig()
            {
                Issuer = _configuration["Token:Issuer"] ?? "n2n",
                Audience = _configuration["Token:Audience"] ?? "*",
                ValidFrom = DateTime.UtcNow,
                ValidTill = n2nTokenRecord.TokenExpirationDate,
                Claims = tokenClaims
            };
            var token = this.GetToken(tokenConfig);

            return token;
        }

        public JwtSecurityToken GetN2NRefreshToken(Guid userId, string nickname, out DateTime expirationDate)
        {
            var lifetime = 43200;
            lifetime = int.TryParse(_configuration["RefreshToken:Lifetime"], out lifetime) ? lifetime : 43200;

            expirationDate = DateTime.UtcNow.AddMinutes(lifetime);

            var n2nRefreshTokenRecord = new N2NRefreshToken()
            {
                Id = Guid.NewGuid(),
                N2NUserId = userId,
                TokenExpirationDate = expirationDate
            };
            _refreshTokenRepo.Add(n2nRefreshTokenRecord);
            var refreshTokenClaims = GetClaims(nickname, n2nRefreshTokenRecord.Id);
            var refreshTokenConfig = new N2NTokenConfig()
            {
                Issuer = _configuration["Token:Issuer"] ?? "n2n",
                Audience = _configuration["Token:Audience"] ?? "*",
                ValidFrom = DateTime.UtcNow,
                ValidTill = n2nRefreshTokenRecord.TokenExpirationDate,
                Claims = refreshTokenClaims
            };

            var refreshToken = this.GetToken(refreshTokenConfig);
            return refreshToken;
        }

        private IEnumerable<Claim> GetClaims(string nickname, Guid tokenId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, nickname),
                new Claim("JWT ID", tokenId.ToString())
            };
            return claims;
        }

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
