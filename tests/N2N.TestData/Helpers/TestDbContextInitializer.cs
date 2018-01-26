﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using N2N.Api.Configuration;
using N2N.Api.Services;
using N2N.Core.Constants;
using N2N.Core.Entities;
using N2N.Infrastructure.DataContext;
using N2N.Infrastructure.Models;
using N2N.TestData;

namespace N2N.Api.Tests.Helpers
{
    public class TestDbContextInitializer
    {
        //
        private Random _random = new Random();
        private N2NDataContext _context;
        private N2NUser[] _users;


        public async Task SeedData(IServiceProvider services)
        {
            // because we have service permission checks 
            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("N2N User Registration Service"), new[] { N2NRoles.Admin });
            
            var appConfigurator = new AppConfigurator();
            appConfigurator.InitRolesAndUsers(services);

            _context = services.GetService<N2NDataContext>();
            var apiUserSrv = services.GetService<N2NApiUserService>();

            //create users, create promises for users
            _users = N2NUsersList.GetList().ToArray();

            foreach (var user in _users)
            {
                var result = await apiUserSrv.CreateUserAsync(user, "Password@123", new[] {N2NRoles.User});
                if (!result.Success)
                {
                    throw new Exception(string.Concat(result.Messages));
                }
                AddPromises(user);
                AddPostcards(user);

                _context.SaveChanges();
            }
        }

        private void AddPromises(N2NUser user)
        {
            var numberOfPromises = _random.Next(0, 4);
            for (int i = 0; i < numberOfPromises; i++)
            {
                var promise = new N2NPromise()
                {
                    Id = Guid.NewGuid(),
                    N2NUserId = user.Id,
                    DueDate = DateTime.UtcNow.AddMonths(_random.Next(1, 6)),
                    IsPublic = _random.Next(0, 4) == 1 ? true : false, // make promise public with given probobility
                    Text = $"TEEEEEEXT for user [{user.NickName}] and promise [{i}]"
                };
                _context.Promises.Add(promise);

                // make generated promise to another user with given probobility
                if (_random.Next(0, 2) == 1)
                {
                    var toUser = new PromiseToUser()
                    {
                        PromiseId = promise.Id,
                        ToUserId = _users[_random.Next(0, _users.Length - 1)].Id,
                        IsFulfilled = _random.Next(0,1) == 1,
                        FulfillDate = DateTime.UtcNow.AddMonths(1)
                    };
                    _context.PromisesToUsers.Add(toUser);
                }
            }
        }


        private void AddPostcards(N2NUser user)
        {
            var numberOfPostcards = _random.Next(0, 4);
            for (int i = 0; i < numberOfPostcards; i++)
            {
                _context.Postcards.Add(PostcardsList.GetPostcard(user.Id));
            }
        }

    }
}
