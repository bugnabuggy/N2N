using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using N2N.Api.Configuration;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;

namespace N2N.Api.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private UserManager<N2NIdentityUser> _userManager;
        private IRepository<N2NToken> _tokenRepo;
        
        public AuthentificationService(UserManager<N2NIdentityUser> userManager, IRepository<N2NToken> tokenRepo)
        {
            this._userManager = userManager;
            this._tokenRepo = tokenRepo;
        }

        public string GetNameUser(string tokenString)
        {
            N2NIdentityUser user= new N2NIdentityUser();
            var tokenClaims = new List<Claim>();
            var jwt = tokenString.Split(' ')[1];
            if (jwt != "")
            {
                tokenClaims = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.ToList();   
            }
            return tokenClaims[0].Value;
        }

        public OperationResult ValidateTokenString(string tokenString)
        {
            var tokenClaims = new List<Claim>();
            var result = new OperationResult();
            try
            {
                var jwt = tokenString.Split(' ')[1];
                
                if (jwt != "")
                {
                    tokenClaims = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.ToList();
                }
                
                if (_tokenRepo.Data.Any(x => x.Id.ToString() == tokenClaims.Find(y => y.Type == "Token Id").Value) &&
                    _tokenRepo.Data.Any( z => z.TokenExpirationDate > DateTime.Now))
                {
                    result.Success = true;
                }
                else
                {
                    result.Messages = new[] { "you do not have authorization token" };
                }

            }
            catch (Exception exp)
            {
                result.Messages = new [] { exp.Message };
            }
            
            return result;
        }

        public void DeleteToken(string tokenString)
        {
            var tokenClaims = new List<Claim>();
            var jwt = tokenString.Split(' ')[1];
            if (jwt != "")
            {
                tokenClaims = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.ToList();
                _tokenRepo.Delete(_tokenRepo.Data.Where(x=>x.Id.ToString()== tokenClaims.Find(y => y.Type == "Token Id").Value));
            }
           
        }

        public async Task<ClaimsIdentity> GetIdentity(string nickName, string password)
        {
            var access = await this._userManager.CheckPasswordAsync(this._userManager.Users.FirstOrDefault(x => x.UserName == nickName), password);
            if (access)
            {
                
                string roleforToken;
                var user = await this._userManager.FindByNameAsync(nickName);
                var role = await this._userManager.IsInRoleAsync(user, "Admin");
                var tokenId = Guid.NewGuid();
                _tokenRepo.Add(new N2NToken
                {
                    Id = tokenId,
                    N2NUserId = user.N2NUserId,
                    TokenExpirationDate = DateTime.Now.AddMinutes(TokenConfig.LIFETIME)
                });
                if (role != true)
                {
                    roleforToken = "User";
                }
                else
                {
                    roleforToken = "Admin";
                }
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, roleforToken),
                        new Claim("Token Id",tokenId.ToString() )
                    };
                    ClaimsIdentity claimsIdentity =
                        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                            ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
            }
            // если пользователя не найдено
            return null;
        }

        public async Task<object> GetTokenObject(ClaimsIdentity identity,Guid userId)
        {
            var now = DateTime.Now;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: TokenConfig.ISSUER,
                audience: TokenConfig.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(TokenConfig.LIFETIME)),
                signingCredentials: new SigningCredentials(TokenConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            var response = new
            {
                access_token = encodedJwt,
            };
            return response;
        }
    }
}
