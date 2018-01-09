using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Services
{
    public interface IN2NPromiseService
    {
        N2NPromise Add(N2NPromise promise);
        N2NPromise Update(N2NPromise promise);
        N2NPromise Delete(Guid promiseId);


    }
}
