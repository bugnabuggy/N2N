using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Core.Constants;
using N2N.Core.DBEntities;
using N2N.Core.Entities;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;

namespace N2N.TestData.Helpers
{
    public class TestDbContextInitializer
    {
        private static bool _isInitialized = false;
        private Random _random = new Random();
        private N2NDataContext _context;
        private N2NUser[] _users;


        public async Task SeedData(IServiceProvider services)
        {
            //if (TestDbContextInitializer._isInitialized)
            //{
            //    return;
            //}

            //TestDbContextInitializer._isInitialized = true;

            // because we have service permission checks, principal should be an admin
            
            var httpContextAccessor = services.GetService<IHttpContextAccessor>();
            var principal = new ClaimsPrincipal(httpContextAccessor.HttpContext.User);
            httpContextAccessor.HttpContext.User = N2NSystem.GetN2NSystemPrincipal();


            var appConfigurator = new AppConfigurator();
            appConfigurator.InitRolesAndUsers(services);

            _context = services.GetService<N2NDataContext>();
            var apiUserSrv = services.GetService<N2NApiUserService>();

            var _userManager = services.GetService<UserManager<N2NIdentityUser>>();
            await _userManager.CreateAsync(N2N.TestData.N2NUsersList.GetN2NIdentityUser(), HardCoddedConfig.DefaultPassword);

            //create users, create promises for users
            _users = N2NUsersList.GetList().ToArray();

            foreach (var user in _users)
            {
                var result = await apiUserSrv.CreateUserAsync(user, HardCoddedConfig.DefaultPassword, new[] {N2NRoles.User});
                if (!result.Success)
                {
                    throw new Exception(string.Concat(result.Messages));
                }
            }

            //to avoid foreing keys insert conflicts
            foreach (var user in _users)
            {
                AddPromises(user, _context);
                AddPostcards(user, _context);
                AddAddressess(user, _context);
            }

            //return default principal back;
            httpContextAccessor.HttpContext.User = principal;
        }

        public void AddPromises(N2NUser user, N2NDataContext context)
        {
            //var numberOfPromises = _random.Next(1, 4);
            var numberOfPromises = 3;
            for (int i = 0; i < numberOfPromises; i++)
            {
                var promise = new N2NPromise()
                {
                    Id = Guid.NewGuid(),
                    N2NUserId = user.Id,
                    DueDate = DateTime.UtcNow.AddMonths(_random.Next(1, 6)),
                    IsPublic = _random.Next(0, 2) == 1 ? true : false, // make promise public with given probobility
                    Text = $"TEEEEEEXT for user [{user.NickName}] and promise [{i}]"
                };
                context.Promises.Add(promise);

                // make generated promise to another user with given probobility
                //if (_random.Next(0, 2) == 1)
                if (i % 2 == 1)
                {
                    var toUser = new PromiseToUser()
                    {
                        PromiseId = promise.Id,
                        ToUserId = _users[_random.Next(0, _users.Length - 1)].Id,
                        IsFulfilled = _random.Next(0,1) == 1,
                        FulfillDate = DateTime.UtcNow.AddMonths(1)
                    };
                    context.PromisesToUsers.Add(toUser);
                }
                context.SaveChanges();
            }
            
        }


        public void AddPostcards(N2NUser user, N2NDataContext context)
        {
            //var numberOfPostcards = _random.Next(1, 2);
            var numberOfPostcards = 2;
            for (int i = 0; i < numberOfPostcards; i++)
            {
                var postcard = PostcardsList.GetPostcard(user.Id);
                var address = AddressList.GetAddress(user);

                context.Postcards.Add(postcard);
                context.Addresses.Add(address);

                var postcardAddress = new PostcardAddress()
                { 
                    AddressId =  address.Id,
                    PostcardId = postcard.Id
                };
                context.PostcardAddresseses.Add(postcardAddress);
            }
            context.SaveChanges();
        }

        public void AddAddressess(N2NUser user, N2NDataContext context)
        {
            //var addrssesNum = _random.Next(1, 2);
            var addrssesNum = 2;
            for (int i = 0; i < addrssesNum; i++)
            {
                var address = AddressList.GetAddress(user);
                context.Addresses.Add(address);
                context.SaveChanges();

                var userAddress = new UserAddress() {AddressId = address.Id, N2NUserId = user.Id};
                context.UserAddresseses.Add(userAddress);
                context.SaveChanges();
            }
            
        }
    }
}
