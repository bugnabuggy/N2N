﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using N2N.Infrastructure.DataContext;
using System;

namespace N2N.Infrastructure.Migrations
{
    [DbContext(typeof(N2NDataContext))]
    partial class N2NDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("N2N.Core.DBEntities.PostcardAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressId");

                    b.Property<Guid>("PostcardId");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("PostcardId");

                    b.ToTable("PostcardAddresseses");
                });

            modelBuilder.Entity("N2N.Core.DBEntities.UserAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressId");

                    b.Property<Guid>("N2NUserId");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("N2NUserId");

                    b.ToTable("UserAddresseses");
                });

            modelBuilder.Entity("N2N.Core.Entities.N2NAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<Guid>("N2NUserId");

                    b.Property<string>("PostalCode");

                    b.HasKey("Id");

                    b.HasIndex("N2NUserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("N2N.Core.Entities.N2NPromise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BlockChainTransaction");

                    b.Property<DateTime?>("DueDate");

                    b.Property<bool>("IsPublic");

                    b.Property<Guid>("N2NUserId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("N2NUserId");

                    b.ToTable("Promises");
                });

            modelBuilder.Entity("N2N.Core.Entities.N2NRefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("N2NUserId");

                    b.Property<DateTime>("RefreshTokenExpirationDate");

                    b.HasKey("Id");

                    b.ToTable("N2NRefreshTokens");
                });

            modelBuilder.Entity("N2N.Core.Entities.N2NToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("IdRefreshToken");

                    b.Property<Guid>("N2NUserId");

                    b.Property<DateTime>("TokenExpirationDate");

                    b.HasKey("Id");

                    b.ToTable("N2NTokens");
                });

            modelBuilder.Entity("N2N.Core.Entities.N2NUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("NickName");

                    b.Property<string>("PhoneNumber");

                    b.Property<DateTime>("Registration");

                    b.Property<string>("UserPic");

                    b.HasKey("Id");

                    b.ToTable("N2NUsers");
                });

            modelBuilder.Entity("N2N.Core.Entities.Postcard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("N2NUserId");

                    b.Property<string>("Picture");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("N2NUserId");

                    b.ToTable("Postcards");
                });

            modelBuilder.Entity("N2N.Core.Entities.PromiseToUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("FulfillDate");

                    b.Property<bool>("IsFulfilled");

                    b.Property<Guid>("PromiseId");

                    b.Property<Guid>("ToUserId");

                    b.HasKey("Id");

                    b.HasIndex("PromiseId");

                    b.HasIndex("ToUserId")
                        .IsUnique();

                    b.ToTable("PromisesToUsers");
                });

            modelBuilder.Entity("N2N.Infrastructure.Models.N2NIdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<Guid>("N2NUserId");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("N2N.Infrastructure.Models.N2NIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("N2N.Infrastructure.Models.N2NIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("N2N.Infrastructure.Models.N2NIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("N2N.Infrastructure.Models.N2NIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("N2N.Core.DBEntities.PostcardAddress", b =>
                {
                    b.HasOne("N2N.Core.Entities.N2NAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("N2N.Core.Entities.Postcard", "Postcard")
                        .WithMany()
                        .HasForeignKey("PostcardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("N2N.Core.DBEntities.UserAddress", b =>
                {
                    b.HasOne("N2N.Core.Entities.N2NAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("N2N.Core.Entities.N2NUser", "User")
                        .WithMany()
                        .HasForeignKey("N2NUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("N2N.Core.Entities.N2NAddress", b =>
                {
                    b.HasOne("N2N.Core.Entities.N2NUser", "User")
                        .WithMany()
                        .HasForeignKey("N2NUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("N2N.Core.Entities.N2NPromise", b =>
                {
                    b.HasOne("N2N.Core.Entities.N2NUser", "User")
                        .WithMany()
                        .HasForeignKey("N2NUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("N2N.Core.Entities.Postcard", b =>
                {
                    b.HasOne("N2N.Core.Entities.N2NUser", "User")
                        .WithMany()
                        .HasForeignKey("N2NUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("N2N.Core.Entities.PromiseToUser", b =>
                {
                    b.HasOne("N2N.Core.Entities.N2NPromise", "Promise")
                        .WithMany()
                        .HasForeignKey("PromiseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("N2N.Core.Entities.N2NUser", "ToUser")
                        .WithOne()
                        .HasForeignKey("N2N.Core.Entities.PromiseToUser", "ToUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
