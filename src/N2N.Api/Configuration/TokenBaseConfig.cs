using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using N2N.Infrastructure.Models;

namespace N2N.Api.Configuration
{
    public class TokenBaseConfig
    {
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }
        public int LIFETIME { get; set; }

        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public virtual async Task<DataForConfiguration> GetConfig(N2NIdentityUser user, Guid tokenId)
        {
            var lifeTime = DateTime.Now.AddMinutes(this.LIFETIME);
            DataForConfiguration config = new DataForConfiguration()
            {
                Audience = this.AUDIENCE,
                Issuer = this.ISSUER,
                TimeLife = lifeTime
            };
            return  new DataForConfiguration();
        }
    }
}
