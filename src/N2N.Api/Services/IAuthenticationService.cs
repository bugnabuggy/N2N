using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using N2N.Core.Models;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;

namespace N2N.Api.Services
{
    public interface IAuthenticationService
    {
        Task<OperationResult<AuthenticationResponseDTO>> AuthenticateUserAsync(string nickName, string password);
        Task<IEnumerable<string>> GetUserRolesAsync(string nickname);

        OperationResult GetUserByTokenString(string tokenString);
        OperationResult AuthenticateByToken(string authorizationHeader);

        void DeleteToken(string tokenString);
        string GetUserName(string tokenString);
        
    }
}
