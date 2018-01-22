using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Interfaces;

namespace N2N.Core.Entities
{
    public class N2NAddress : IOwned
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }

        public Guid N2NUserId { get; set; }
        [ForeignKey("N2NUserId")]
        public N2NUser User { get; set;}
    }
}
