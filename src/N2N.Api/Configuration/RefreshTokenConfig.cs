using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Test;
using Microsoft.IdentityModel.Tokens;

namespace N2N.Api.Configuration
{
    public class RefreshTokenConfig : IConfig
    {
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }
        public int LIFETIME { get; set; }

        public RefreshTokenConfig()
        {
            this.ISSUER = "MyAuthServer"; // издатель токена
            this.AUDIENCE = "*";
            this.LIFETIME = 30; // время жизни токена (30 дней рекомендуемое)
        }
    }
}
