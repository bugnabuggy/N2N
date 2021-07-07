using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Infrastructure.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using System.Reflection;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace N2N.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private ILoggerFactory _loggerFactory;
        private AppConfigurator appConfigurator = new AppConfigurator();


        public Startup(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<N2NDataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            services.AddIdentity<N2NIdentityUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequiredLength = 4;
                    })
                .AddEntityFrameworkStores<N2NDataContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(IdentityServer.GetApis())
                .AddInMemoryClients(IdentityServer.GetClients())
                .AddAspNetIdentity<N2NIdentityUser>()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                            sql => sql.MigrationsAssembly(typeof(N2NDataContext).GetTypeInfo().Assembly.GetName().Name));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });


            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });


            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(options =>
                {

                    options.Authority = Configuration["ServerUrl"];
                    options.RequireHttpsMetadata = false;

                    options.ApiName = SiteConstants.ApiName;
                });

            appConfigurator.ConfigureServices(services);

            var cors = new DefaultCorsPolicyService(_loggerFactory.CreateLogger<DefaultCorsPolicyService>())
            {
                AllowAll = true,
            };
            services.AddSingleton<ICorsPolicyService>(cors);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();

            appConfigurator.UseMvcAndConfigureRoutes(app);

            app.UseIdentityServer();

        }

    }
}
