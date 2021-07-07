using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using N2N.Api.Configuration;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;

namespace N2N.TestData.Helpers
{
    public class DatabaseDiBootstrapperInMemory : IServiceProviderBootstrapper
    {
        public N2NDataContext GetDataContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<N2NDataContext>();
            optionsBuilder.UseInMemoryDatabase("TestInMemory");
            var ctx = new N2NDataContext(optionsBuilder.Options);
            return ctx;
        }

        public ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<N2NDataContext>(options => options.UseInMemoryDatabase("Test"));

            services.AddIdentity<N2NIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<N2NDataContext>();

            var httpContext = new DefaultHttpContext();
            httpContext.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            services.AddTransient<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = httpContext });

            var appConfigurator = new AppConfigurator();
            appConfigurator.ConfigureServices(services);

            services.AddSingleton<IConfiguration>(c => new Mock<IConfiguration>().Object);

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
