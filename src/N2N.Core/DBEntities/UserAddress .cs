using System;
using System.ComponentModel.DataAnnotations.Schema;
using N2N.Core.Entities;
using N2N.Core.Interfaces;

namespace N2N.Core.DBEntities
{
    /// <summary>
    /// Address that user creates as associated with him
    /// </summary>
    public class UserAddress : IOwned
    {
        public int Id { get; set; }

        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public N2NAddress Address { get; set; }

        public Guid N2NUserId { get; set; }
        [ForeignKey("N2NUserId")]
        public N2NUser User { get; set; }
    }
}
