using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Infrastructure.Exceptions
{
    public class N2NSecurityException : Exception
    {
        public N2NSecurityException(string message):base(message)
        {

        }
    }
}
