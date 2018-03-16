using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Isam.Esent.Interop;
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
        private IRepository<N2NUser> _userRepo;
        private IRepository<N2NToken> _tokenRepo;
        private IN2NTokenService _tokenService;
        private IConfiguration _configuration;
        

        public AuthenticationService(UserManager<N2NIdentityUser> userManager,
            IRepository<N2NUser> userRepo,
            IRepository<N2NToken> tokenRepo,
            IN2NTokenService tokenService,
            IConfiguration configuration)
        {

            this._userManager = userManager;
            this._userRepo = userRepo;
            this._tokenRepo = tokenRepo;
            this._tokenService = tokenService;
            this._configuration = configuration;
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

        public OperationResult GetUserByTokenString(string tokenString)
        {
            throw new NotImplementedException();
        }

        public OperationResult<N2NUser> AuthenticateByTokenString(string jwt)
        {
            var result = new OperationResult<N2NUser>() {Messages = new List<string>()};
            var tokenClaims = new List<Claim>();
            JwtSecurityToken token;

            if (string.IsNullOrWhiteSpace(jwt))
            {
                result.Messages.Add("Token string is empty");
                goto end;
            }
            
            token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            tokenClaims = token.Claims.ToList();

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

            var n2nToken = _tokenRepo.Data.FirstOrDefault(t => t.Id.ToString() == token.Id);
            if (n2nToken == null)
            {
                result.Messages.Add("Token record not found");
                goto end;
            }

            var user = _userRepo.Data.FirstOrDefault(u => u.Id == n2nToken.N2NUserId);
            if (user == null)
            {
                result.Messages.Add("Token is valid, but no associated user found in database");
                goto end;
            }

            result.Data = user;
            result.Success = true;

            end:
            return result;
        }

        public OperationResult<N2NUser> AuthenticateByAuthHeader(string authorizationHeader)
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
                    result = this.AuthenticateByTokenString(jwt);

                }
                catch (Exception exp)
                {
                    result.Messages.Add("Your Authorization header is corruped or do not use Bearer scheme");
                    if(!string.IsNullOrEmpty(exp.Message)) {result.Messages.Add(exp.Message);}
                }
            }
            return result;
        }

        public OperationResult RefreshAccessToken(string refreshTokenString)
        {
            throw new NotImplementedException();
        }

        public void DeleteToken(string tokenString)
        {
            throw new NotImplementedException();
        }

        public string GetUserName(string tokenString)
        {
            throw new NotImplementedException();
        }
    }
}
