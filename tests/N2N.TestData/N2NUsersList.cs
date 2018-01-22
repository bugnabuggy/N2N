using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.TestData
{
    public class N2NUsersList
    {
        public static IEnumerable<N2NUser> GetList()
        {
            return new List<N2NUser>()
            {
                new N2NUser()
                {
                    Id = Guid.Parse("1594587A-8943-46B6-A398-DA06D01113BA"),
                    NickName = "An"
                }
            };
        }
    }
}
