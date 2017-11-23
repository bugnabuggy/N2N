using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Infrastructure.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }
}
