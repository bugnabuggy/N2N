using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
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
        //ToDo
        public string GetUserName(string tokenString)
        {
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

            if (string.IsNullOrEmpty(authorizationHeader))
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

        public async Task<OperationResult<AuthenticationResponseDTO>> AuthenticateUserAsync(string nickName, string password)
        {
            AuthenticationResponseDTO response = null;
            var  tokenConfig = new TokenConfig(_tokenRepo);
            var access_token = await GetToken(tokenConfig,nickName, password);
            if (access_token != "")
            {
                var refreshTokenConfig = new RefreshTokenConfig(_RefreshTokenRepo);
                var refresh_token = await GetToken(refreshTokenConfig, nickName, password);
                response = new AuthenticationResponseDTO
                {
                    access_token = access_token,
                    // TODO: Add expiration_date 
                    refresh_token = refresh_token
                };
            }
            return new OperationResult<AuthenticationResponseDTO>(){ Data = response};
        }

        public async Task<string> GetToken(TokenBaseConfig config, string nickName, string password)
        {
            var tokenId = Guid.NewGuid();
            var user = await this._userManager.FindByNameAsync(nickName);  
            DataForConfiguration tokenConfig = await config.GetConfig(user, tokenId);
            tokenConfig.Identity = await GetClaimsIdentity(nickName, password, tokenId);
            var token = await GetTokenObject(tokenConfig);

            return token;
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

        public async Task<string> GetTokenObject(DataForConfiguration tokenConfing) 
        {

            string response="";
            
            var now = DateTime.Now;
                var jwt = new JwtSecurityToken(
                    issuer: tokenConfing.Issuer,
                    audience: tokenConfing.Audience,
                    notBefore: now,
                    claims: tokenConfing.Identity.Claims,
                    expires: tokenConfing.TimeLife,
                    signingCredentials: new SigningCredentials(TokenConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                response = encodedJwt;
           
            
            return response;
        }
    }
}
