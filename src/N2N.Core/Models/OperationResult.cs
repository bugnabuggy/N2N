using System.Collections.Generic;

namespace N2N.Core.Models
{

    public abstract class OperationResultBase
    {
        public OperationResultBase()
        {
            this.Messages = new List<string>();
        }

        public bool Success { get; set; }
        public IList<string> Messages { get; set; }
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
