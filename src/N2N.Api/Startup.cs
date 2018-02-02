﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using N2N.Data.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;


namespace N2N.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private AppConfigurator appConfigurator = new AppConfigurator();

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //var tokenConfig = new TokenConfig();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            // укзывает, будет ли валидироваться издатель при валидации токена
            //            ValidateIssuer = true,
            //            // строка, представляющая издателя
            //            ValidIssuer = tokenConfig.ISSUER,

            //            // будет ли валидироваться потребитель токена
            //            ValidateAudience = true,
            //            // установка потребителя токена
            //            ValidAudience = tokenConfig.AUDIENCE,
            //            // будет ли валидироваться время существования
            //            ValidateLifetime = true,

            //            // установка ключа безопасности
            //            IssuerSigningKey = TokenConfig.GetSymmetricSecurityKey(),
            //            // валидация ключа безопасности
            //            ValidateIssuerSigningKey = true,
            //            ClockSkew = TimeSpan.Zero

            //        };
            //    });

            services.AddDbContext<N2NDataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            services.AddIdentity<N2NIdentityUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequireDigit = true;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 5;
                    })
                .AddEntityFrameworkStores<N2NDataContext>()
                .AddDefaultTokenProviders();

            appConfigurator.ConfigureServices(services);

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

            app.UseAuthentication();
            
        }

    }
}
