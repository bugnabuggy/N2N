using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Entities
{
    public class UserDataForm
    {
        public string NickName { get; set; }
        public string Password { get; set; }
        public string Capcha { get; set; }
    }
}
