﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Data.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using N2N.Services;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace N2N.Api.Configuration
{
    public static class AppStart
    {
        internal static void UseMvcAndConfigureRoutes(IApplicationBuilder app)
        {
            app.UseCors("AllowSpecificOrigin");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=N2N}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Integrates Simple injector to App, used in Startup calss 
        /// </summary>
        /// <param name="services">Services from Startup class</param>
        /// <param name="container">DI container</param>
        internal static void IntegrateSimpleInjector(IServiceCollection services, Container container)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }


        /// <summary>
        /// Wire up dependecy injection
        /// </summary>
        /// <param name="app"></param>
        /// <param name="container"></param>
        internal static void InitializeContainer(IApplicationBuilder app, Container container)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Cross wire Identity services
            container.CrossWire<N2NDataContext>(app);
            container.CrossWire<UserManager<N2NIdentityUser>>(app);
            container.CrossWire<SignInManager<N2NIdentityUser>>(app);
            container.CrossWire<IAuthentificationService>(app);
            
            // Dependencies
            container.Register<IRepository<N2NUser>, DbRepository<N2NUser>>();
            container.Register<IRepository<N2NToken>, DbRepository<N2NToken>>();
            container.Register<IRepository<N2NRefreshToken>, DbRepository<N2NRefreshToken>>();
            container.Register<ISecurityService, SecurityService>();
            container.Register<IN2NUserService, N2NUserService>();
            container.Register<N2NApiUserService>();

        }

        internal static bool BootstrapDb(N2NDataContext ctx)
        {
            var result = ctx.Database.EnsureCreated();
            //ctx.Database.Migrate();

            return result;
        }
    }
}
