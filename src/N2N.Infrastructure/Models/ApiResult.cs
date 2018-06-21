using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Infrastructure.Models
{
    public class ApiResultBase
    {
        public int Count { get; set; }
        public int PageSize { get; set; }
    }

    public class ApiResult : ApiResultBase
    {
        public IEnumerable<object> Data { get; set; }
    }

    public class ApiResult<T> : ApiResultBase
    {
        public IEnumerable<T> Data { get; set; }
    }

}
