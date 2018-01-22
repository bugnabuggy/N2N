using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Core.Entities;
using N2N.Core.Services;
using N2N.Data.Repositories;
using N2N.Infrastructure.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace N2N.Api.Tests
{
    [TestFixture()]
    class AppConfiguratorTests
    {

        [Test]
        public async Task Should_init_default_users_and_roles()
        {
            var appConfigurator = new AppConfigurator();
            var serviceProvider = await new DbAndSupportClassesBootstrapper().GetServiceProvider();

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("N2N Test Service"), new[] { "Admin" });

            appConfigurator.InitRolesAndUsers(serviceProvider);

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<N2NIdentityUser>>();
            var n2nUsersRepo = serviceProvider.GetRequiredService<IRepository<N2NUser>>();

            Assert.IsTrue(roleManager.Roles.Any(x=>x.Name.Equals("User")));
            Assert.IsTrue(roleManager.Roles.Any(x => x.Name.Equals("Admin")));

            Assert.IsTrue(userManager.Users.Any(x=>x.UserName.Equals("admin")));
            Assert.IsTrue(userManager.Users.Any(x => x.UserName.Equals("Administrator")));
            Assert.IsTrue(userManager.Users.Any(x => x.UserName.Equals("jokero")));

            Assert.IsTrue(n2nUsersRepo.Data.Count() >= 3);

            var jUser = userManager.Users.FirstOrDefault(x => x.UserName.Equals("jokero"));
            Assert.IsTrue(userManager.IsInRoleAsync(jUser,"Admin").Result);
        }

    }
}
