using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.TestData
{
    public class AddressList
    {
        public static N2NAddress GetAddress(N2NUser user)
        {
            return new N2NAddress()
            {
                AddressLine1 = $"Chepaeva street 111 {user.Email}",
                AddressLine2 = $"office 205 {user.NickName}",
                Country = "Russia",
                City = "Omsk",
                PostalCode = "644007",
                N2NUserId = user.Id
            };
        }

    }
}
