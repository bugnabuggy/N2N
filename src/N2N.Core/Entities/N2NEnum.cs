using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Entities
{
    public class N2NEnum
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Value { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public N2NEnumCategory Category { get; set; }
        
    }
}
