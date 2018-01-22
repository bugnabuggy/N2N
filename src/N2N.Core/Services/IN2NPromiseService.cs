using System;
using N2N.Core.Entities;

namespace N2N.Core.Services
{
    public interface IN2NPromiseService
    {
        N2NPromise Add(N2NPromise promise);
        N2NPromise Update(N2NPromise promise);
        N2NPromise Delete(Guid promiseId);
    }
}
