using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace N2N.Infrastructure.Models
{
    public class N2NIdentityUser : IdentityUser
    {
        public Guid N2NUserId { get; set; }
    }
}
