using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using N2N.Infrastructure.Models;

namespace N2N.Api.Services
{
    public interface IAuthentificationService
    {
        OperationResult ValidateTokenString(string tokenString);

        void DeleteToken(string tokenString);
        Task<object> Authentification(string nickName, string password);
        string GetNameUser(string tokenString);
    }
}
