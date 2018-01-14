using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Interfaces;

namespace N2N.Core.Entities
{
    public class Postcard : IOwned
    {
        public Guid Id { get; set; }

        public string Picture { get; set; }
        public string Text { get; set; }

        public Guid N2NUserId { get; set; }
    }
}
