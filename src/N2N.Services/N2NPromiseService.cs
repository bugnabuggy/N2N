using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.Exceptions;
using N2N.Infrastructure.Models;

namespace N2N.Services
{
    public class N2NPromiseService : IN2NPromiseService
    {
        private IRepository<N2NPromise> _promiseRepo;
        private ISecurityService _securitySrv;


        public N2NPromiseService(IRepository<N2NPromise> promiseRepo, ISecurityService securitySrv)
        {
            _promiseRepo = promiseRepo;
            _securitySrv = securitySrv;
        }

        public N2NPromise Add(N2NPromise promise)
        {
            if (promise.N2NUserId == _securitySrv.GetCurrentN2NUserId())
                return _promiseRepo.Add(promise);
            else
            {
                //even admins should not be able to make promises on behalf another
                throw new N2NSecurityException("User can't add promise for another user");
            }
        }

        public N2NPromise Update(N2NPromise promise)
        {
            if (promise.N2NUserId == _securitySrv.GetCurrentN2NUserId() ||
                _securitySrv.HasAccess())
            {
                return _promiseRepo.Update(promise);
            }
            else
            {
                //even admins should not be able to make promises on behalf another
                throw new N2NSecurityException("User can't edit another user's promise if he is not in administrator role");
            }
        }

        public N2NPromise Delete(Guid promiseId)
        {
            var promise = _promiseRepo.Data.FirstOrDefault(p => p.Id == promiseId);

            if(promise == null) throw new NotFoundException(promiseId.ToString());

            if (promise.N2NUserId == _securitySrv.GetCurrentN2NUserId() ||
                _securitySrv.HasAccess())
            {
                return _promiseRepo.Delete(promise);
            }
            else
            {
                throw new N2NSecurityException("User can't edit another user's promise if he is not in administrator role");
            }
        }
    }
}
