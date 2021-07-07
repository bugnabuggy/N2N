using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Interfaces
{
    public interface IN2NToken
    {
        Guid Id { get; set; }
        Guid N2NUserId { get; set; }
        DateTime TokenExpirationDate { get; set; }
    }
}
