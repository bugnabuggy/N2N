using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.TestData.Helpers
{
    public class HardCoddedConfig
    {
        public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=n2n-test;Trusted_Connection=True;MultipleActiveResultSets=true";
        public const int AsyncOperationWaitTime = 10_000;
    }
}
