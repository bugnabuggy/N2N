using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2N.Api.Configuration
{
    interface IConfig
    {
        string ISSUER { get; set; }
        string AUDIENCE { get; set; }
        int LIFETIME { get; set; }
    }
}
