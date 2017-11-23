using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Data.Repositories
{
    public class TestClass
    {
        private IRepository<N2NUser> _repo;

        public TestClass(N2N.Data.Repositories.IRepository<N2NUser> repo)
        {
            this._repo = repo;
        }

        public string Test()
        {
            return "Hello 123! " + _repo.Data.Count(x => x.NickName != "");
        }
    }
}
