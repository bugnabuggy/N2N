using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Entities
{
    public class N2NUser
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public DateTime Registration { get; set; }
    }
}
