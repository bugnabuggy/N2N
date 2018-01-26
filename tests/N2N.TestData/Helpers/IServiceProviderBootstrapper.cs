using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Infrastructure.DataContext;

namespace N2N.TestData.Helpers
{
    public interface IServiceProviderBootstrapper
    {
        N2NDataContext GetDataContext();

        ServiceProvider GetServiceProvider();
        ServiceProvider GetServiceProviderWithSeedDB();
    }
}
