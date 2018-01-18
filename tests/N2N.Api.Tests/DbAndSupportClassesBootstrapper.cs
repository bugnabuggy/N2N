using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Core.Services;
using N2N.Data.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Api.Tests
{
    public class DbAndSupportClassesBootstrapper
    {
        public async Task<ServiceProvider> GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<N2NDataContext>(options => options.UseInMemoryDatabase("Test"));

            services.AddIdentity<N2NIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<N2NDataContext>();
            var context = new DefaultHttpContext();

            context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = context });



            //TODO: integrate simple injector or remove it for the api project to make one DI configurator
            // var appConfigurator = new AppConfigurator(); 

            services.AddTransient<IRepository<N2NToken>, DbRepository<N2NToken>>();
            services.AddTransient<IRepository<N2NUser>, DbRepository<N2NUser>>();
            services.AddTransient<IN2NUserService, N2NUserService>();
            services.AddTransient<ISecurityService, SecurityService>();
            
            services.AddTransient<N2NApiUserService>();

            var serviceProvider = services.BuildServiceProvider();

            var userManager = serviceProvider.GetRequiredService<UserManager<N2NIdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //create identity user, but not a n2n user
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            var testUser = new N2NIdentityUser() { UserName = "An" };
            await userManager.CreateAsync(testUser);
            await userManager.AddToRoleAsync(testUser, "Admin");

            return serviceProvider;
        }
    }
}
