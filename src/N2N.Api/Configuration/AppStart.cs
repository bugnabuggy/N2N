using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing.Constraints;

namespace N2N.Api.Configuration
{
    public static class AppStart
    {
        public static void UseMvcAndConfigureRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=N2N}/{action=Index}/{id?}");
            });
        }
    }
}
