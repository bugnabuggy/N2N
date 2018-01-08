using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace N2N.Infrastructure.DataContext
{
    class N2NDataContextFactory : IDesignTimeDbContextFactory<N2NDataContext>
    {
        public N2NDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<N2NDataContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=n2n-dev;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new N2NDataContext(optionsBuilder.Options);
        }
    }
}
