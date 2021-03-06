using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;



namespace N2N.Web
{
  public class Startup
  {
   
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseStaticFiles();

      var shellPath = env.WebRootPath + "/" + Configuration["Shell"];
      var shellContent = this.GetShellContent(shellPath);

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync(shellContent);
      });
    }

    private string GetShellContent(string shellPath)
    {
      return File.ReadAllText(shellPath);
    }
  }
}
