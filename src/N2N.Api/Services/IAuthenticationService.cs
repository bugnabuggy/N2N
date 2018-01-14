using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;

namespace N2N.Api.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponseDTO> AuthenticateUser(string nickName, string password);

        OperationResult GetUserByTokenString(string tokenString);
        OperationResult AuthenticateByToken(string authorizationHeader);

        void DeleteToken(string tokenString);
        string GetUserName(string tokenString);
    }
}
