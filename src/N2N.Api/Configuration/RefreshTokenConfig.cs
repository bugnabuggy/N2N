using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Test;
using Microsoft.IdentityModel.Tokens;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;

namespace N2N.Api.Configuration
{
    public class RefreshTokenConfig: TokenBaseConfig
    {
        private IRepository<N2NRefreshToken> _refreshTokenRepo;

        public RefreshTokenConfig(IRepository<N2NRefreshToken> refreshTokenRepo)
        {
            this._refreshTokenRepo = refreshTokenRepo;
            this.ISSUER = "MyAuthServer"; // издатель токена
            this.AUDIENCE = "*";
            this.LIFETIME = 30; // время жизни токена (30 дней рекомендуемое)
        }

        public override async Task<DataForConfiguration> GetConfig(N2NIdentityUser user, Guid tokenId)
        {
            var lifeTime = DateTime.Now.AddDays(this.LIFETIME);
            DataForConfiguration config = new DataForConfiguration()
            {
                Audience = this.AUDIENCE,
                Issuer = this.ISSUER,
                TimeLife = lifeTime
            };
            _refreshTokenRepo.Add(new N2NRefreshToken
            {
                Id = tokenId,
                N2NUserId = user.N2NUserId,
                TokenExpirationDate = lifeTime
            });
            return config;
        }
    }
}
