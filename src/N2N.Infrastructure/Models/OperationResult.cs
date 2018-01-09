using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Infrastructure.Models
{

    public abstract class OperationResultBase
    {
        public bool Success { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }

    public class OperationResult: OperationResultBase
    {
        public object Data { get; set; }
    }

    public class OperationResult<T> : OperationResultBase
    {
        public T Data { get; set; }
    }
}
