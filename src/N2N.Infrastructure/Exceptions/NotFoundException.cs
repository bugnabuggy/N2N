using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Infrastructure.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ItemId { get; set; }

        public NotFoundException(string Id):base($"Item  with id = [{Id}] not found")
        {
            ItemId = Id;
        }
        
    }
}
