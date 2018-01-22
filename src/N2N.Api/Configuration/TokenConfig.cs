using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace N2N.Api.Configuration
{
    public class TokenConfig :IConfig
    {
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }
        public int LIFETIME { get; set; }

        public  TokenConfig()
        {
            this.ISSUER = "MyAuthServer"; // издатель токена
            this.AUDIENCE = "*";
            this.LIFETIME = 60; // время жизни токена (180 минут рекомендуемое)
        }
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
