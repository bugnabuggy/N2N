using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using N2N.Infrastructure.DataContext;

namespace N2N.Infrastructure.Tests.Helpers
{
     static class DataContextHelper
    {
        private static int _contextCount = 0;
        private static DbContextOptions<N2NDataContext> _options;


        static DataContextHelper( )
        {
            var settings = ConfigurationManager.AppSettings;
            
            var optionsBuilder = new DbContextOptionsBuilder<N2NDataContext>();

            if (settings[0] == "true")
            {
                optionsBuilder.UseInMemoryDatabase(settings[1]);

            }
            else
            {
                optionsBuilder.UseSqlServer(settings[2]);
            }



            _options = optionsBuilder.Options;
        }
        public static N2NDataContext GetDataContext()
        {
            var ctx = new N2NDataContext(_options);

            if (_contextCount < 1)
            {
                ctx.Database.EnsureCreated();
            }

            _contextCount++;
            return ctx;
        }

        public static bool disposeDataContext(N2NDataContext ctx)
        {
            if (_contextCount < 2)
            {
                ctx.Database.EnsureDeleted();
            }
            ctx.Dispose();

            _contextCount--;
            return true;
        }
    }
}
