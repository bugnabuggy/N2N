using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using N2N.Api.Configuration;
using N2N.Core.Entities;
using N2N.Core.Models;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;

namespace N2N.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private UserManager<N2NIdentityUser> _userManager;
        private IRepository<N2NToken> _tokenRepo;
        private IRepository<N2NRefreshToken> _RefreshTokenRepo;
        private IRepository<N2NUser> _userRepo;


        public AuthenticationService( UserManager<N2NIdentityUser> userManager,
                                        IRepository<N2NToken> tokenRepo,
                                        IRepository<N2NRefreshToken> refreshTokenRepo,
                                        IRepository<N2NUser> userRepo)
        {
            this._userManager = userManager;
            this._tokenRepo = tokenRepo;
            this._RefreshTokenRepo = refreshTokenRepo;
            this._userRepo = userRepo;
        }

        public string GetUserName(string tokenString)
        {
            N2NIdentityUser user = new N2NIdentityUser();
            var tokenClaims = new List<Claim>();
            var jwt = tokenString.Split(' ')[1];
            if (jwt != "")
            {
                tokenClaims = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.ToList();   
            }

            return tokenClaims[0].Value;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string nickname)
        {
            var identityUser = await _userManager.FindByNameAsync(nickname);
            var roles = await _userManager.GetRolesAsync(identityUser);
            return roles;
        }

        public OperationResult GetUserByTokenString(string tokenString)
        {
            var tokenClaims = new List<Claim>();
            var result = new OperationResult();
            result.Messages = new List<string>();

            try
            {
                var jwt = tokenString.Split(' ')[1];
                
                if (jwt != "")
                {
                    tokenClaims = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.ToList();
                }
                var token = _tokenRepo.Data.FirstOrDefault(t => t.Id.ToString() == tokenClaims.Find(y => y.Type == "Token Id").Value);

                if (token != null)
                {
                    if (token?.TokenExpirationDate > DateTime.Now)
                    {
                        var user = _userRepo.Data.FirstOrDefault(x => x.Id == token.N2NUserId);

                        result.Data = user;
                        result.Success = true;
                    }
                    else
                    {
                        (result.Messages as List<string>).Add("Authorization token has expired");
                    }
                }
                else
                {
                    (result.Messages as List<string>).Add("Token not found");
                }

            }
            catch (Exception exp)
            {
                result.Messages = new [] { exp.Message };
            }
            
            return result;
        }

        public OperationResult AuthenticateByToken(string authorizationHeader)
        {
            var messages = new List<string>();
            var success = false;
            object data = null;

            if (authorizationHeader.IsNullOrEmpty())
            {
                messages.Add("You do not have Authorization header");
            }
            else
            {
                var tokenValidationResult = this.GetUserByTokenString(authorizationHeader);
                if (tokenValidationResult.Success)
                {
                    if (tokenValidationResult.Data?.GetType() == typeof(N2NUser))
                    {
                        data = tokenValidationResult.Data as N2NUser;
                        success = true;

                        // methdod shoud not affect global scope!!! ↓ so comment it out
                        //Thread.CurrentPrincipal =
                        //    new GenericPrincipal(
                        //        new N2NIdentity(tokenValidationResult.Data as N2NUser, isAuthenticated: true),
                        //        new string[] { });

                    }
                    else
                    {
                        messages.Add("User have not been found in token validation data");
                    }
                }
                else
                {
                    messages.AddRange(tokenValidationResult.Messages);
                }
            }

            return new OperationResult()
            {
                Messages = messages,
                Success = success,
                Data = data
            };
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

        public async Task<AuthenticationResponseDTO> AuthenticateUser(string nickName, string password)
        {
            AuthenticationResponseDTO response = null;
            var access_token = await GetToken(nickName, password);
            if (access_token !="")
            {
                var refresh_token = await GetRefreshToken(nickName, password);
                response= new AuthenticationResponseDTO
                {
                    access_token = access_token,
                    // TODO: Add expiration_date 
                    refresh_token = refresh_token
                };
            }
            return response;
        }

        public async Task<string> GetToken(string nickName, string password)
        {
            var token = "";
            var tokenConfig = new TokenConfig();
            var lifeTime = TimeSpan.FromMinutes(tokenConfig.LIFETIME);
            var user = await this._userManager.FindByNameAsync(nickName);
            var tokenId = Guid.NewGuid();
            
            var claimIdentity = await GetClaimsIdentity(nickName, password, tokenId);
            if (claimIdentity != null)
            {
                _tokenRepo.Add(new N2NToken
                {
                    Id = tokenId,
                    N2NUserId = user.N2NUserId,
                    TokenExpirationDate = DateTime.Now.AddMinutes(tokenConfig.LIFETIME)
                });
                token = await GetTokenObject(claimIdentity, user.N2NUserId, tokenConfig.ISSUER,
                    tokenConfig.AUDIENCE,
                    lifeTime);
            }
            return token;
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
            var claimIdentity = await GetClaimsIdentity(nickName, password, tokenId);
            var refreshToken =await GetTokenObject(claimIdentity, user.N2NUserId, tokenConfig.ISSUER, tokenConfig.AUDIENCE,
                lifeTime);
            return  refreshToken;
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(string nickName, string password, Guid tokenId)
        {
            var user = await this._userManager.FindByNameAsync(nickName);
            if (user != null)
            {
                var access = await this._userManager.CheckPasswordAsync(user, password);
                if (access)
                {
                    var isAdmin = await this._userManager.IsInRoleAsync(user, "Admin");
                    var roleForToken = isAdmin != true ? "User" : "Admin";


                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, roleForToken),
                        new Claim("Token Id", tokenId.ToString() )
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
