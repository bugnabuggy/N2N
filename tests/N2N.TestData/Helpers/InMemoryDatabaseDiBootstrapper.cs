using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Api.Tests.Helpers;
using N2N.Core.Entities;
using N2N.Core.Services;
using N2N.Data.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using N2N.TestData.Helpers;

namespace N2N.Api.Tests
{
    public class InMemoryDatabaseDiBootstrapper : IServiceProviderBootstrapper
    {
        public ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<N2NDataContext>(options => options.UseInMemoryDatabase("Test"));

            services.AddIdentity<N2NIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<N2NDataContext>();

            var httpContext = new DefaultHttpContext();
            httpContext.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = httpContext });

            var appConfigurator = new AppConfigurator();
            appConfigurator.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        public ServiceProvider GetServiceProviderWithSeedDB()
        {
            var provider = GetServiceProvider();
            var dbSeed = new TestDbContextInitializer();
            dbSeed.SeedData(provider);

            return provider;
        }
        
    }
}
