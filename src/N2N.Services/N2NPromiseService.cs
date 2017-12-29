using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;


namespace N2N.Services
{
    public class N2NPromiseService : IN2NPromiseService
    {
        private IRepository<N2NPromise> _promiseRepo;
        private ISecurityService _security;

        public N2NPromiseService(IRepository<N2NPromise> promiseRepo, ISecurityService security)
        {
            this._promiseRepo = promiseRepo;
            this._security = security;
        }

        public OperationResult GetPromise(Guid promiseId)
        {
            var result = new OperationResult();
            var promise=_promiseRepo.Data.FirstOrDefault(x => x.Id == promiseId);
            result.Data = promise;
            result.Success = true;
            result.Messages = new[] { $"Promise was created with Id = ${promise.Id}" };
            return result;
        }

        public OperationResult CreatePromise(N2NPromise promise)
        {
            var result = new OperationResult();

            if (this._security.HasAccess())
            {
                
                _promiseRepo.Add(promise);

                result.Data = promise;
                result.Success = true;
                result.Messages = new[] { $"Promise was created with Id = ${promise.Id}" };
            }
            else
            {
                result.Messages = new[] { "You dont have permissions to create promise" };
            }

            return result;
        }

        public OperationResult UpdatePromise(N2NPromise promise)
        {
            var result = new OperationResult();

            if (this._security.HasAccess())
            {
                //add user here
                var idPromise = _promiseRepo.Data.FirstOrDefault(x => x.Id == promise.Id);
                promise.N2NUserId = idPromise.N2NUserId;
                _promiseRepo.Update(promise);

                result.Data = promise;
                result.Success = true;
                result.Messages = new[] { $"Promise was created with Id = ${promise.Id}" };
            }
            else
            {
                result.Messages = new[] { "You dont have permissions to create promise" };
            }

            return result;
        }

        public OperationResult DeletePromise(Guid idPromise)
        {
            var result = new OperationResult();
            if (this._security.HasAccess())
            {
                //add user here
                var promise = _promiseRepo.Data.FirstOrDefault(x => x.Id == idPromise);
                _promiseRepo.Delete(promise);

                result.Data = null;
                result.Success = true;
                result.Messages = new[] { $"Promise was delete"};
            }
            else
            {
                result.Messages = new[] { "not found promise" };
            }
            return result;
        }
    }
}
