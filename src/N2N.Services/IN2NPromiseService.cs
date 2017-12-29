using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;

namespace N2N.Services
{
    public interface IN2NPromiseService
    {
        OperationResult CreatePromise(N2NPromise promise);
        OperationResult UpdatePromise(N2NPromise promise);
        OperationResult DeletePromise(Guid idPromise);
    }
}
