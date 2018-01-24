using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.TestData.Helpers
{
    public interface IServiceProviderBootstrapper
    {
        ServiceProvider GetServiceProvider();
        ServiceProvider GetServiceProviderWithSeedDB();
    }
}
