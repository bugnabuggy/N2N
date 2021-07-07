using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Core.DBEntities
{
    public class PostcardAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public N2NAddress Address { get; set; }


        public Guid PostcardId { get; set; }
        [ForeignKey("PostcardId")]
        public Postcard Postcard { get; set; }
    }
}
