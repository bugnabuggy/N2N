using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Repositories;

namespace N2N.TestData
{
    public class AthorizationMocks
    {
        private static string _token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiVG9rZW4gSWQiOiIyMDM0MzMxYi0yMTcwLTRjN2EtYTU1YS02YWNkYmJkMjg4ZTgiLCJuYmYiOjE1MTU2NDc0NTUsImV4cCI6MTUxNTY1MTA1NSwiaXNzIjoiTXlBdXRoU2VydmVyIiwiYXVkIjoiKiJ9.ad1l_YapU0Nh4yB4K4gkphXTU3VwP1ktV540gDy9ND4";

        public static string GetTokenString()
        {
            return AthorizationMocks._token;
        }

        public static string GetAuthorizationHeaderTokenString()
        {
            return "Bearer " + AthorizationMocks._token;
        }

        public static string GetExpiredRefreshToken(IRepository<N2NRefreshToken> refreshTokenRepo, IN2NTokenService tokenService)
        {
            var refreshToken = new N2NRefreshToken()
            {
                Id = Guid.NewGuid(),
                N2NUserId = TestData.N2NUsersList.GetList().ToArray()[0].Id,
                TokenExpirationDate = DateTime.UtcNow.AddDays(-60)
            };
            refreshTokenRepo.Add(refreshToken);
            var token = tokenService.GetToken(new N2NTokenConfig()
            {
                Claims =  new List<Claim>() { new Claim("jti", refreshToken.Id.ToString())},
                ValidFrom = DateTime.UtcNow.AddDays(-90),
                ValidTill = DateTime.UtcNow.AddDays(-60)
            });

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
