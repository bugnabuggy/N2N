using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Configuration;
using N2N.Api.Tests.Helpers;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;

namespace N2N.TestData.Helpers
{
    public class DatabaseDiBootstrapperSQLServer : IServiceProviderBootstrapper
    {
        private static int _contextCount = 0;
        private static DbContextOptions<N2NDataContext> _options;
        // not for resharper or vs studio test runners, have to be separat test runner project! to use config
        //private static NameValueCollection _settings = ConfigurationManager.AppSettings;


        static DatabaseDiBootstrapperSQLServer( )
        {
            var optionsBuilder = new DbContextOptionsBuilder<N2NDataContext>();
            optionsBuilder.UseSqlServer(HardCoddedConfig.ConnectionString);
            _options = optionsBuilder.Options;
        }

        public static N2NDataContext GetDataContext()
        {
            var ctx = new N2NDataContext(_options);

            if (_contextCount < 1)
            {
                //clean befor new test session starts
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }

            _contextCount++;
            return ctx;
        }

        public static bool DisposeDataContext(N2NDataContext ctx)
        {
            if (_contextCount < 2)
            {
                ctx.Database.EnsureDeleted();
            }
            ctx.Dispose();

            _contextCount--;
            return true;
        }

        N2NDataContext IServiceProviderBootstrapper.GetDataContext()
        {
            return GetDataContext();
        }

        public ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddDbContext<N2NDataContext>(options => options.UseSqlServer(HardCoddedConfig.ConnectionString));

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

        public async Task<ServiceProvider> GetServiceProviderWithSeedDB()
        {
            var provider = GetServiceProvider();
            var dbSeed = new TestDbContextInitializer();
            await dbSeed.SeedData(provider);

            return provider;
        }
    }
}
