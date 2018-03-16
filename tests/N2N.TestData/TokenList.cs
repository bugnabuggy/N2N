using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.TestData.Helpers;

namespace N2N.TestData
{
    public class TokenList
    {

        public static string GetTokenString(N2NUser user, IN2NTokenService tokenSvc)
        {
            var token = tokenSvc.GetN2NToken(user.Id, user.NickName, Guid.Empty, out DateTime expiration);
            return new JwtSecurityTokenHandler().WriteToken(token) ;
        }

        public static List<N2NToken> GetList()
        {
            return new List<N2NToken>
            {
                new N2NToken()
                {
                    Id = Guid.Parse("2034331b-2170-4c7a-a55a-6acdbbd288e8"),
                    N2NUserId = Guid.Parse("1594587A-8943-46B6-A398-DA06D01113BA"),
                    TokenExpirationDate =  DateTime.Now.AddDays(1),
                    RefreshTokenId = Guid.Parse("A56852A4-C68A-464C-AFB7-C33225FB8F6E")
                },
                new N2NToken() {
                    Id = Guid.Parse("A3D2A357-C914-9AA9-CF90-C8BA28B7D357"),
                    N2NUserId = Guid.Parse("b91d9fde-578a-40d8-b103-d490a04ba11a"),
                    TokenExpirationDate = DateTime.Parse("2017-11-10 11:10:55.4290685"),
                    RefreshTokenId = Guid.Parse("D43DF6B3-AF5E-343A-5E50-E8C5E1F3D86C")
                }
            };
        }
    }
}
