using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2N.Api.Configuration
{
    public class SiteConstants
    {
        public const int AsyncTaskWaitTime = 10_000;
        public const string ApiName = "site api";
        public const int AccessTokenLifeTime = 60 * 60 * 24 * 30;
    }
}
