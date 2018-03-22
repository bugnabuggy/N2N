using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Isam.Esent.Interop;
using N2N.Api.Configuration;
using N2N.Core.Entities;
using N2N.Core.Interfaces;
using N2N.Core.Models;
using N2N.Infrastructure.Repositories;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;

namespace N2N.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private UserManager<N2NIdentityUser> _userManager;
        private IRepository<N2NUser> _userRepo;
        private IRepository<N2NToken> _tokenRepo;
        private IRepository<N2NRefreshToken> _refreshTokenRepo;
        private IN2NTokenService _tokenService;
        

        public AuthenticationService(UserManager<N2NIdentityUser> userManager,
            IRepository<N2NUser> userRepo,
            IRepository<N2NToken> tokenRepo,
            IRepository<N2NRefreshToken> refreshTokenRepo,
            IN2NTokenService tokenService
            )
        {

            this._userManager = userManager;
            this._userRepo = userRepo;
            this._tokenRepo = tokenRepo;
            this._refreshTokenRepo = refreshTokenRepo;
            this._tokenService = tokenService;
        }

        public async Task<OperationResult<LoginResponseDTO>> LoginUserAsync(string nickName, string password)
        {
            var result = new OperationResult<LoginResponseDTO>();

            //check credentials 
            var user = await this._userManager.FindByNameAsync(nickName);
            if (user == null)
            {
                result.Messages.Add("Password is wrong or user not exists");
                goto end;
            }

            //authentication
            var authResult = await _userManager.CheckPasswordAsync(user, password);
            if (!authResult)
            {
                result.Messages.Add("Password is wrong or user not exists");
                goto end;
            }

            var n2nUser = _userRepo.Data.FirstOrDefault(u => u.NickName.Equals(nickName));
            if (n2nUser == null)
            {
                result.Messages.Add("Identity user exists but N2NUser not found in the database");
                goto end;
            }

            try
            {
                //generate tokens        
                DateTime refreshTokenExpirationDate;
                var refreshToken = _tokenService.GetN2NRefreshToken(n2nUser.Id, n2nUser.NickName, out refreshTokenExpirationDate);
                
                DateTime tokenExpirationDate;
                var token = _tokenService.GetN2NToken(n2nUser.Id, n2nUser.NickName, Guid.Empty, out tokenExpirationDate);
                

                result.Success = true;
                result.Data = new LoginResponseDTO()
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration_date = tokenExpirationDate,
                    refresh_token = new JwtSecurityTokenHandler().WriteToken(refreshToken)
                };
            }
            catch (Exception exp)
            {
                //add logging here
                throw exp;
            }

            //return result
            end:
            return result;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string nickname)
        {
            var identityUser = await _userManager.FindByNameAsync(nickname);
            var roles = await _userManager.GetRolesAsync(identityUser);
            return roles;
        }

        public async Task<OperationResult<N2NUser>> AuthenticateByTokenStringAsync(string jwt)
        {
            var result = new OperationResult<N2NUser>() {Messages = new List<string>()};

            if (string.IsNullOrWhiteSpace(jwt))
            {
                result.Messages.Add("Token string is empty");
                goto end;
            }
            try
            {
                JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

                if (token.ValidTo < DateTime.UtcNow)
                {
                    result.Messages.Add("Token has expired");
                    goto end;
                }

                if (token.ValidFrom > DateTime.UtcNow)
                {
                    result.Messages.Add("Token effective date has not come yet");
                    goto end;
                }

                var n2nToken = await _tokenRepo.Data.FirstOrDefaultAsync(t => t.Id.ToString() == token.Id);
                if (n2nToken == null)
                {
                    result.Messages.Add("Token record not found");
                    goto end;
                }

                var user = await _userRepo.Data.FirstOrDefaultAsync(u => u.Id == n2nToken.N2NUserId);
                if (user == null)
                {
                    result.Messages.Add("Token is valid, but no associated user found in database");
                    goto end;
                }

                result.Data = user;
                result.Success = true;
            }
            catch (Exception exp)
            {
                result.Messages.Add("Token string is corrupted");
                result.Messages.Add(exp.Message);
            }

            end:
            return result;
        }

        public async Task<OperationResult<N2NUser>> AuthenticateByAuthHeaderAsync(string authorizationHeader)
        {
            var result = new OperationResult<N2NUser>(){Messages = new List<string>()};
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                result.Messages.Add("You do not have Authorization header");
            }
            else
            {
                try
                {
                    var parts = authorizationHeader.Split(' ');
                    var schema = parts[0];
                    var jwt = parts[1];
                    if (! schema.Equals("Bearer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new Exception("");
                    }
                    result = await this.AuthenticateByTokenStringAsync(jwt);

                }
                catch (Exception exp)
                {
                    result.Messages.Add("Your Authorization header is corruped or do not use Bearer scheme");
                    if(!string.IsNullOrEmpty(exp.Message)) {result.Messages.Add(exp.Message);}
                }
            }
            return result;
        }

        public async Task<OperationResult<AccessTokenDTO>> RefreshAccessTokenAsync(string refreshTokenString)
        {
            var result = new OperationResult<AccessTokenDTO>() {Messages = new List<string>()};

            try
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenString);

                var tokenId = Guid.Parse(token.Id);
                var n2nRefreshToken = await _refreshTokenRepo.Data.FirstOrDefaultAsync(x => x.Id.Equals(tokenId));

                if (n2nRefreshToken == null)
                {
                    result.Messages.Add($"Refresh token with id [{tokenId}] not found in database");
                    goto end;
                }

                if (n2nRefreshToken.TokenExpirationDate < DateTime.UtcNow)
                {
                    result.Messages.Add($"Refresh token has expired");
                    goto end;
                }

                //remove all effective access tokens created with this refresh token
                var accessTokens = await _tokenRepo.GetAsync(x => x.RefreshTokenId.Equals(tokenId) && x.TokenExpirationDate > DateTime.UtcNow, null, null);
                //var accessTokens = _tokenRepo.Data.Where(x => x.RefreshTokenId.Equals(tokenId) && x.TokenExpirationDate > DateTime.UtcNow);
                _tokenRepo.Delete(accessTokens);

                var nickName = token.Claims.FirstOrDefault(x => x.Type.Equals(ClaimsIdentity.DefaultNameClaimType))?.Value;

                var accessToken = _tokenService.GetN2NToken(n2nRefreshToken.N2NUserId,
                    nickName,
                    tokenId,
                    out DateTime expirationDate
                );

                var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

                result.Success = true;
                result.Data = new AccessTokenDTO()
                {
                    access_token = accessTokenString,
                    expiration_date = expirationDate
                };

            }
            catch (Exception exp)
            {
                result.Messages.Add("Refresh token string is corrupted");
                result.Messages.Add(exp.Message);
            }

            end:
            return result;
        }

        public async Task<OperationResult<N2NToken>> DeleteAccessToken(string tokenString)
        {
            return await this.DeleteTokenAsync<N2NToken>(tokenString, this._tokenRepo);
        }

        public async Task<OperationResult<N2NRefreshToken>> DeleteRefreshToken(string tokenString)
        {
            return await this.DeleteTokenAsync<N2NRefreshToken>(tokenString, this._refreshTokenRepo);
        }

        public async Task<OperationResult<T>> DeleteTokenAsync<T>(string tokenString, IRepository<T> _repo) where T :  class, IN2NToken
        {
            var result = new OperationResult<T>() {Messages = new List<string>()};
            try
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
                var tokenId = Guid.Parse(token.Id);

                var n2nToken = await _repo.Data.FirstOrDefaultAsync(x => x.Id.Equals(tokenId));
                _repo.Delete(n2nToken);

                result.Success = true;
                result.Data = n2nToken;
            }
            catch (Exception exp)
            {
                result.Messages.Add(exp.Message);    
            }

            return result;
        }

        public async Task<string> GetUserNameAsync(string tokenString)
        {
            var user = await this.AuthenticateByTokenStringAsync(tokenString);
                    
            return user.Data.NickName;
        }
    }
}
