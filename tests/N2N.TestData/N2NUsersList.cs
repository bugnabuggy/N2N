using System;
using System.Collections.Generic;
using N2N.Core.Entities;
using N2N.Infrastructure.Models;

namespace N2N.TestData
{
    public class N2NUsersList
    {
        public static N2NIdentityUser GetN2NIdentityUser()
        {
            return new N2NIdentityUser()
            {
                UserName = "NoN2N"
            };
        }

        public static N2NUser GetNotInDbUser()
        {
            return new N2NUser()
            {
                Id = Guid.NewGuid(),
                NickName = "User",
                Registration = DateTime.UtcNow,
                Email = "test@test.xcom",
                FirstName = "Te",
                LastName = "St",
                PhoneNumber = "+123456789"
            };
        }

        public static IEnumerable<N2NUser> GetList()
        {
            return new List<N2NUser>()
            {
                new N2NUser()
                {
                    Id = Guid.Parse("1594587A-8943-46B6-A398-DA06D01113BA"),
                    NickName = "An",
                    Registration = DateTime.UtcNow,
                },

                new N2NUser()
                {
                    Id = Guid.NewGuid(),
                    NickName = "Test1",
                    Registration = DateTime.UtcNow,
                    Email = "test1@test.xcom",
                    FirstName = "Te",
                    LastName = "St",
                    PhoneNumber = "+123456789"
                },
                new N2NUser()
                {
                    Id = Guid.NewGuid(),
                    NickName = "Test2",
                    Registration = DateTime.UtcNow,
                    Email = "test2@test.xcom",
                    FirstName = "Te 2",
                    LastName = "St ",
                    PhoneNumber = "+123456789"
                },
                new N2NUser()
                {
                    Id = Guid.NewGuid(),
                    NickName = "Test3",
                    Registration = DateTime.UtcNow,
                    Email = "test3@test.xcom",
                    FirstName = "Te 3",
                    LastName = "St",
                    PhoneNumber = "+123456789"
                }
            };
        }
    }
}
