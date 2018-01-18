using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Core.Interfaces;

namespace N2N.Core.DBEntities
{
    public class UserAddresses : IOwned
    {
        public Guid Id { get; set; }

        public Guid AddressId { get; set; }
        [ForeignKey("AddressId")]
        public N2NAddress Address { get; set; }

        public Guid N2NUserId { get; set; }
        [ForeignKey("N2NUserId")]
        public N2NUser User { get; set; }
    }
}
