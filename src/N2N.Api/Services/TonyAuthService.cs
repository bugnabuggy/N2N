using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using N2N.Api.Configuration;
using N2N.Core.Entities;
using N2N.Core.Models;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;

namespace N2N.Api.Services
{
    public class TonyAuthService : IAuthenticationService
    {
        private UserManager<N2NIdentityUser> _userManager;
        private IRepository<N2NUser> _userRepo;
        private IN2NTokenService _tokenService;
        private IConfiguration _configuration;
        

        public TonyAuthService(UserManager<N2NIdentityUser> userManager,
            IRepository<N2NUser> userRepo,
            IN2NTokenService tokenService,
            IConfiguration configuration)
        {

            this._userManager = userManager;
            this._userRepo = userRepo;
            this._tokenService = tokenService;
            this._configuration = configuration;
        }

        public async Task<OperationResult<AuthenticationResponseDTO>> AuthenticateUserAsync(string nickName, string password)
        {
            var result = new OperationResult<AuthenticationResponseDTO>();

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
                result.Data = new AuthenticationResponseDTO()
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

        public OperationResult<N2NUser> AuthenticateByTokenString(string token)
        {
            throw new NotImplementedException();
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
