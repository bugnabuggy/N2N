using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Infrastructure.Exceptions
{
    public class N2NException : Exception
    {
        public Guid N2NUserId { get; set; }
    }
}
