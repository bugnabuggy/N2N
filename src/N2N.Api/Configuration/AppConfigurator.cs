using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Services;
using N2N.Core.Constants;
using N2N.Core.Entities;
using N2N.Core.Services;
using N2N.Data.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using N2N.Services;

namespace N2N.Api.Configuration
{
    public class AppConfigurator //have to make it public because want to test it
    {
        internal void UseMvcAndConfigureRoutes(IApplicationBuilder app)
        {
            app.UseCors("AllowSpecificOrigin");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=N2N}/{action=Index}/{id?}");
            });
        }

        public void InitRolesAndUsers(IServiceProvider services)
        {
            const int WAIT_TIME = 10_000;
            var roles = new List<IdentityRole>()
            {
                new IdentityRole(N2NRoles.Admin),
                new IdentityRole(N2NRoles.User)
            };

            var users = new Dictionary<N2NUser, string>()
            {
                {new N2NUser(){NickName = "jokero"}, "Password@123"},
                {new N2NUser(){NickName = "admin"}, "Password@123"},
                {new N2NUser(){NickName = "Administrator"}, "Password@123"}
            };

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userService = services.GetRequiredService<N2NApiUserService>();

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role.Name).Result)
                {
                    roleManager.CreateAsync(role).Wait(WAIT_TIME);
                }
            }

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal( new GenericIdentity("N2N system initialization"), new []{ N2NRoles.Admin } );

            foreach (var user in users)
            {
                if (!userService.UserExistsAndConsistentAsync(user.Key.NickName).Result)
                {
                    userService.CreateUserAsync(user.Key, user.Value, new []{ N2NRoles.User, N2NRoles.Admin }).Wait(WAIT_TIME);
                }
            }
        }

        /// <summary>
        /// Wire up dependecy injection
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository<N2NRefreshToken>, DbRepository<N2NRefreshToken>>();
            services.AddTransient<IRepository<N2NToken>, DbRepository<N2NToken>>();
            services.AddTransient<IRepository<N2NUser>, DbRepository<N2NUser>>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddTransient<IRepository<N2NUser>, DbRepository<N2NUser>>();
            services.AddTransient<IRepository<N2NToken>, DbRepository<N2NToken>>();
            services.AddTransient<IRepository<N2NRefreshToken>, DbRepository<N2NRefreshToken>>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IN2NUserService, N2NUserService>();

            services.AddTransient<N2NApiUserService>();

        }
    }
}
