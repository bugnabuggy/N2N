using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using N2N.Api.Configuration;
using N2N.Infrastructure.DataContext;

namespace N2N.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            var appConfigurator = new AppConfigurator();

            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var db = services.GetRequiredService<N2NDataContext>();
                    db.Database.Migrate();

                    appConfigurator.InitRolesAndUsers(services);

                    var identityServerDb = services.GetRequiredService<PersistedGrantDbContext>();
                    identityServerDb.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            Utilities.Logging.Logger.LogWarning("Api site is starting");
            webHost.Run();
            
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    logging.AddConsole(options => options.IncludeScopes = true);
                //    logging.AddDebug();
                //})
                .Build();
    }
}
