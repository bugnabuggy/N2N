using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Infrastructure.Models.DTO
{
    public class N2NPostcardDTO
    {
        public Postcard Postcard { get; set; }
        public IEnumerable<N2NAddress> Addresses { get; set; }
    }
}
