using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Core.Interfaces;
using N2N.Core.Models;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;
using N2N.Infrastructure.Repositories;

namespace N2N.Api.Services
{
    public interface IAuthenticationService
    {
        Task<OperationResult<LoginResponseDTO>> LoginUserAsync(string nickName, string password);
        Task<IEnumerable<string>> GetUserRolesAsync(string nickname);

        Task<OperationResult<N2NUser>> AuthenticateByTokenStringAsync(string token);
        Task<OperationResult<N2NUser>> AuthenticateByAuthHeaderAsync(string authorizationHeader);
        Task<OperationResult<AccessTokenDTO>> RefreshAccessTokenAsync(string refreshTokenString);

        Task<OperationResult<T>> DeleteTokenAsync<T>(string tokenString, IRepository<T> _repo) where T : class, IN2NToken;
        Task<string> GetUserNameAsync(string tokenString);
        
    }
}
