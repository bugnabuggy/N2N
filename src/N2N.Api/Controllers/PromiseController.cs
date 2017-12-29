using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("Promise")]
    public class PromiseController : Controller
    {
        private IRepository<N2NUser> _userRepo;
        private IN2NPromiseService _n2NPromiseService;
        private IAuthentificationService _authentificationService;

        public PromiseController(IN2NPromiseService n2NPromiseService, IRepository<N2NUser> userRepo, IAuthentificationService authentificationService)
        {
            this._authentificationService = authentificationService;
            this._userRepo = userRepo;
            this._n2NPromiseService = n2NPromiseService;
        }

        //[HttpGet("{promiseId}")]
        //public async Task<object> GetPromise(string promiseId)
        //{
        //    return 
        //}

        [HttpPost("/Promise/SavePromise")]
        public async Task<object> SavePromiseOnServer([FromBody] PromiseDTO promise)
        {
            OperationResult result = new OperationResult();
            DateTime dueDate =new DateTime() ;
            if (!promise.TextPromise.IsNullOrEmpty())
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                var userId = this._userRepo.Data.FirstOrDefault(x =>
                    x.NickName == this._authentificationService.GetNameUser(authHeader.ToString())).Id;
                if (promise.DataImplementationPromise!="")
                {
                    dueDate = DateTime.ParseExact(promise.DataImplementationPromise, "yyyy-mm-dd", null);
                }
                N2NPromise newPromise= new N2NPromise()
                {
                    Id = Guid.NewGuid(),
                    DueDate = dueDate,
                    Text = promise.TextPromise,
                    IsPublic = promise.IsPublish,
                    N2NUserId = userId,
                    BlockChainTransaction = "",
                    HashIdLinkPromise = ""
                };
                
                result= this._n2NPromiseService.CreatePromise(newPromise);
            }
            
            return result.Data;
        }
    }
}
