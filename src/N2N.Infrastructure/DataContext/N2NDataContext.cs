using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;

namespace N2N.Infrastructure.DataContext
{
    public class N2NDataContext : IdentityDbContext<N2NIdentityUser>
    {
        public N2NDataContext(DbContextOptions<N2NDataContext> options)
            : base(options)
        {
        }

        public DbSet<N2NUser> N2NUsers { get; set; }
        public DbSet<N2NToken> N2NTokens { get; set; }
        public DbSet<N2NRefreshToken> N2NRefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
