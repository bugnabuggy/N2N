﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Test;
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
        private IRepository<N2NRefreshToken> _RefreshTokenRepo;

        
        public AuthentificationService(UserManager<N2NIdentityUser> userManager, IRepository<N2NToken> tokenRepo, IRepository<N2NRefreshToken> RefreshTokenRepo)
        {
            this._userManager = userManager;
            this._tokenRepo = tokenRepo;
            this._RefreshTokenRepo = RefreshTokenRepo;
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

        public async Task<object> Authentification(string nickName, string password)
        {
            object response=  new { };
            var access_token = await GetToken(nickName,password);
            if (access_token !="")
            {
                var refresh_token = await GetRefreshToken(nickName, password);
                response= new
                {
                    access_token = access_token,
                    refresh_token = refresh_token
                };
            }
            return response;
        }

        public async Task<string> GetToken(string nickName, string password)
        {
            var Token="";
            var tokenConfig = new TokenConfig();
            var lifeTime = TimeSpan.FromMinutes(tokenConfig.LIFETIME);
            var user = await this._userManager.FindByNameAsync(nickName);
            var tokenId = Guid.NewGuid();
            
            var claimIdentity = await GetIdentity(nickName, password, tokenId);
            if (claimIdentity != null)
            {
                _tokenRepo.Add(new N2NToken
                {
                    Id = tokenId,
                    N2NUserId = user.N2NUserId,
                    TokenExpirationDate = DateTime.Now.AddMinutes(tokenConfig.LIFETIME)
                });
                Token = await GetTokenObject(claimIdentity, user.N2NUserId, tokenConfig.ISSUER,
                    tokenConfig.AUDIENCE,
                    lifeTime);
            }
            return Token;
        }

        public async Task<string> GetRefreshToken(string nickName, string password)
        {
            var tokenConfig = new RefreshTokenConfig();
            var lifeTime = TimeSpan.FromDays(tokenConfig.LIFETIME);
            var user = await this._userManager.FindByNameAsync(nickName);
            var tokenId = Guid.NewGuid();
            _RefreshTokenRepo.Add(new N2NRefreshToken()
            {
                Id = tokenId,
                N2NUserId = user.N2NUserId,
                RefreshTokenExpirationDate = DateTime.Now.AddDays(tokenConfig.LIFETIME)
            });
            var claimIdentity = await GetIdentity(nickName, password, tokenId);
            var refreshToken =await GetTokenObject(claimIdentity, user.N2NUserId, tokenConfig.ISSUER, tokenConfig.AUDIENCE,
                lifeTime);
            return  refreshToken;
        }

        public async Task<ClaimsIdentity> GetIdentity(string nickName, string password,Guid tokenId)
        {
            var access = await this._userManager.CheckPasswordAsync(this._userManager.Users.FirstOrDefault(x => x.UserName == nickName), password);
            if (access)
            {
                
                string roleforToken;
                var user = await this._userManager.FindByNameAsync(nickName);
                var role = await this._userManager.IsInRoleAsync(user, "Admin");
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

        public async Task<string> GetTokenObject(ClaimsIdentity identity,Guid userId, string issuer , string audience, TimeSpan lifetime)
        {

            string response="";
            
            var now = DateTime.Now;
                var jwt = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(lifetime),
                    signingCredentials: new SigningCredentials(TokenConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                response = encodedJwt;
           
            
            return response;
        }
    }
}
