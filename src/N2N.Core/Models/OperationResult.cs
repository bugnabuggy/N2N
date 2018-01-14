using System.Collections.Generic;

namespace N2N.Core.Models
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
