using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Core.Constants;
using N2N.Core.Entities;
using N2N.Core.Services;
using N2N.Infrastructure.Repositories;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using N2N.TestData.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace N2N.Api.Tests
{
    [TestFixture]
    class AppConfiguratorTests
    {

        [Test]
        public void Should_init_default_users_and_roles()
        {
            var appConfigurator = new AppConfigurator();
            var serviceProvider = new DatabaseDiBootstrapperInMemory().GetServiceProvider();

            appConfigurator.InitRolesAndUsers(serviceProvider);

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<N2NIdentityUser>>();
            var n2nUsersRepo = serviceProvider.GetRequiredService<IRepository<N2NUser>>();

            Assert.IsTrue(roleManager.Roles.Any(x=>x.Name.Equals(N2NRoles.User)));
            Assert.IsTrue(roleManager.Roles.Any(x => x.Name.Equals(N2NRoles.Admin)));

            Assert.IsTrue(userManager.Users.Any(x=>x.UserName.Equals("admin")));
            Assert.IsTrue(userManager.Users.Any(x => x.UserName.Equals("Administrator")));
            Assert.IsTrue(userManager.Users.Any(x => x.UserName.Equals("jokero")));

            Assert.IsTrue(n2nUsersRepo.Data.Count() >= 3);

            var jUser = userManager.Users.FirstOrDefault(x => x.UserName.Equals("jokero"));
            Assert.IsTrue(userManager.IsInRoleAsync(jUser, N2NRoles.Admin).Result);
        }

        [Test]
        public void Should_return_empty_db_for_new_service_provider_and_existing_db_for_existing_provider()
        {
            var serviceProvider = new DatabaseDiBootstrapperInMemory().GetServiceProvider();
            var context = serviceProvider.GetRequiredService<N2NDataContext>();

            Assert.AreEqual(context.Promises.Count(),0);
            Assert.AreEqual(context.N2NUsers.Count(), 0);
            Assert.AreEqual(context.PromisesToUsers.Count(), 0);

            var guid = Guid.NewGuid();
            context.Promises.Add(new N2NPromise(){Id = guid});
            context.N2NUsers.Add(new N2NUser(){Id = guid});
            context.PromisesToUsers.Add(new PromiseToUser(){PromiseId = guid, ToUserId = guid});
            context.SaveChanges();

            //darabase with values from existing service provider
            var context1 = serviceProvider.GetRequiredService<N2NDataContext>();
            Assert.AreEqual(context1.Promises.Count(), 1);
            Assert.AreEqual(context1.N2NUsers.Count(), 1);
            Assert.AreEqual(context1.PromisesToUsers.Count(), 1);

            //clear database for new service provider
            var serviceProvider2 = new DatabaseDiBootstrapperInMemory().GetServiceProvider();
            var context2 = serviceProvider2.GetRequiredService<N2NDataContext>();

            Assert.AreEqual(context2.Promises.Count(), 0);
            Assert.AreEqual(context2.N2NUsers.Count(), 0);
            Assert.AreEqual(context2.PromisesToUsers.Count(), 0);
        }
    }
}
