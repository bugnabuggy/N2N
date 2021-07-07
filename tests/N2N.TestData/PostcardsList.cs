using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.TestData
{
    class PostcardsList
    {
        private static Random _random = new Random();

        private static Guid GetRandomUserId(Guid[] usersIdslist)
        {
            var max = usersIdslist.Length-1;
            Guid guid = usersIdslist[_random.Next(max)];

            return guid;
        }

        public static Postcard GetPostcard(Guid usersId)
        {
            return
                new Postcard()
                {
                    Id = Guid.NewGuid(),
                    N2NUserId = usersId,
                    Text = "some postcard text",
                    Picture = "this is link or base64 picture"
                };

        }

        public static List<Postcard> GetList(Guid[] usersIdslist)
        {
            var list = new List<Postcard>
            {
                new Postcard()
                {
                    Id =  Guid.NewGuid(),
                    N2NUserId = GetRandomUserId(usersIdslist),
                    Text = "some postcard text",
                    Picture = "this is link or base64 picture"
                }
            };
            return list;
        }
    }
}
