using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Core.Models;
using N2N.Core.Services;
using N2N.Infrastructure.Repositories;
using N2N.Infrastructure.Models;
using N2N.Infrastructure.Models.DTO;
using N2N.Services;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("Promise")]
    public class PromiseController : Controller
    {
        private IRepository<N2NUser> _userRepo;
        private IN2NPromiseService _N2NPromiseService;
        private IAuthenticationService _authenticationService;

        public PromiseController(IN2NPromiseService N2NPromiseService, IRepository<N2NUser> userRepo, IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
            this._userRepo = userRepo;
            this._N2NPromiseService = N2NPromiseService;
        }

        //[HttpGet("{promiseId}")]
        //public async Task<object> GetPromise(string promiseId)
        //{
        //    return 
        //}

        [HttpPost]
        public async Task<OperationResult> SavePromiseOnServer([FromBody] PromiseDTO promise)
        {
            OperationResult result = new OperationResult();
            DateTime dueDate =new DateTime() ;
            if (!string.IsNullOrEmpty(promise.TextPromise))
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                var nickname = await this._authenticationService.GetUserNameAsync(authHeader.ToString());
                var userId = this._userRepo.Data.FirstOrDefault(x =>
                    x.NickName == nickname).Id;
                if (promise.DataImplementationPromise != "")
                {
                    dueDate = DateTime.ParseExact(promise.DataImplementationPromise, "yyyy-mm-dd", null);
                }
                N2NPromise newPromise = new N2NPromise()
                {
                    Id = Guid.NewGuid(),
                    DueDate = dueDate,
                    Text = promise.TextPromise,
                    IsPublic = promise.IsPublish,
                    N2NUserId = userId,
                    BlockChainTransaction = "",
                    HashIdLinkPromise = ""
                };

                result.Data = this._N2NPromiseService.Add(newPromise);
                result.Success = true;
            }
            else
            {
                result.Messages = new[] {"Promise text is empty"};
            }

            return result;
        }
    }
}
