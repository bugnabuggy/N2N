using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Entities
{
    public class Postcard
    {
        public Guid Id { get; set; }

        public string Picture { get; set; }
        public string Text { get; set; }
    }
}
