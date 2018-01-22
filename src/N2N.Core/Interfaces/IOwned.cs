using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Interfaces
{
    public interface IOwned
    {
        Guid N2NUserId { get; set; }
    }
}
