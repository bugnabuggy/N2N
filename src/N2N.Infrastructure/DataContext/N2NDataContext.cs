using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using N2N.Core.Entities;
using N2N.Core.DBEntities;
using N2N.Core.Interfaces;
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
        
        public DbSet<N2NPromise> Promises { get; set; }
        public DbSet<PromiseToUser> PromisesToUsers { get; set; }

        public DbSet<Postcard> Postcards{ get; set; }
        public DbSet<N2NAddress> Addresses{ get; set; }
        public DbSet<UserAddress> UserAddresseses{ get; set; }
        public DbSet<PostcardAddress> PostcardAddresseses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<PromiseToUser>()
                .HasOne(x => x.ToUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PromiseToUser>()
                .HasOne(x => x.Promise)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserAddress>()
                .HasOne(a => a.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserAddress>()
                .HasOne(x => x.Address)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PostcardAddress>()
                .HasOne(x => x.Address)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PostcardAddress>()
                .HasOne(x => x.Address)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
