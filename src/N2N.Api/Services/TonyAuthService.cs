using System;
using System.Collections.Generic;
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
        private IRepository<N2NToken> _tokenRepo;
        private IRepository<N2NRefreshToken> _refreshTokenRepo;
        private IRepository<N2NUser> _userRepo;
        private ITokenGenerator _tokenGenerator;
        private IConfiguration _configuration;

        public TonyAuthService(UserManager<N2NIdentityUser> userManager,
            IRepository<N2NUser> userRepo,
            IRepository<N2NToken> tokenRepo,
            IRepository<N2NRefreshToken> refreshTokenRepo,
            ITokenGenerator tokenGenerator,
            IConfiguration configuration)
        {

            this._userManager = userManager;
            this._tokenRepo = tokenRepo;
            this._refreshTokenRepo = refreshTokenRepo;
            this._userRepo = userRepo;
            this._tokenGenerator = tokenGenerator;
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
                //generate token        
                var n2nTokenRecord = new N2NToken()
                {
                    Id = Guid.NewGuid(),
                    N2NUserId = n2nUser.Id,
                    IdRefreshToken = Guid.Empty,
                    TokenExpirationDate = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Token:Lifetime"]))
                };
                var n2nRefreshToken = new N2NRefreshToken()
                {
                    Id = Guid.NewGuid(),
                    N2NUserId = n2nUser.Id
                };
                _tokenRepo.Add(n2nTokenRecord);
                _refreshTokenRepo.Add(n2nRefreshToken);

                var tokenClaims = GetClaims(nickName, n2nTokenRecord.Id);

                var tokenConfig = new N2NTokenConfig();
                var refreshTokenConfig = new N2NTokenConfig();

                var token = _tokenGenerator.GetToken(tokenConfig);
                var refreshToken = _tokenGenerator.GetToken(refreshTokenConfig);

            }
            catch (Exception exp)
            {

            }

            //return result
            end:
            return result;
        }

        private IEnumerable<Claim> GetClaims(string nickname, Guid tokenId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, nickname),
                new Claim("Token Id", tokenId.ToString())
            };
            return claims;
        }

        public Task<IEnumerable<string>> GetUserRolesAsync(string nickname)
        {
            throw new NotImplementedException();
        }

        public OperationResult GetUserByTokenString(string tokenString)
        {
            throw new NotImplementedException();
        }

        public OperationResult AuthenticateByToken(string authorizationHeader)
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
