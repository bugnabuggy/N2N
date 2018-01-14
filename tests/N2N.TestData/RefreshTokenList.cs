using N2N.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.TestData.SupportClasses;

namespace N2N.TestData
{
    public class RefreshTokenList
    {
        public static List<N2NRefreshToken> GetList()
        {
            return new List<N2NRefreshToken>
            {
                new N2NRefreshToken()
                {
                    Id = Guid.Parse("A56852A4-C68A-464C-AFB7-C33225FB8F6E"),
                    N2NUserId = Guid.Parse("1594587A-8943-46B6-A398-DA06D01113BA"),
                    RefreshTokenExpirationDate = DateTime.Parse("2018-02-10 11:10:55.4290685")
                },
                new N2NRefreshToken() {
                    Id = Guid.Parse("515c25ab-9e44-47ce-a301-092090f5e136"),
                    N2NUserId = Guid.Parse("b91d9fde-578a-40d8-b103-d490a04ba11a"),
                    RefreshTokenExpirationDate = DateTime.Parse("2017-11-10 11:10:55.4290685")
                }
            };
        }

    }
}
