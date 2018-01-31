using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;

namespace N2N.Api.Configuration
{
    public class TokenConfig :TokenBaseConfig
    {
       
        private IRepository<N2NToken> _tokenRepo;

        public TokenConfig()
        {
            this.ISSUER = "MyAuthServer"; // издатель токена
            this.AUDIENCE = "*";
            this.LIFETIME = 60;
        }

        public  TokenConfig(IRepository<N2NToken> tokenRepo)
        {
            this._tokenRepo = tokenRepo;
            this.ISSUER = "MyAuthServer"; // издатель токена
            this.AUDIENCE = "*";
            this.LIFETIME = 60;
        }

        public override async Task<DataForConfiguration> GetConfig(N2NIdentityUser user, Guid tokenId)
        {
            var lifeTime = DateTime.Now.AddMinutes(this.LIFETIME);
            DataForConfiguration config = new DataForConfiguration()
            {
                Audience = this.AUDIENCE,
                Issuer = this.ISSUER,
                TimeLife = lifeTime 
            };
            _tokenRepo.Add(new N2NToken
            {
                Id = tokenId,
                N2NUserId = user.N2NUserId,
                TokenExpirationDate = lifeTime
            });
            return  config;
        }
    }
}
