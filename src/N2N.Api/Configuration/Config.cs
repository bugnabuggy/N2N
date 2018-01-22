using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace N2N.Api.Configuration
{
    public class Config
    {
      public DateTime TimeLife { get; set; }
      public ClaimsIdentity Identity { get; set; }
      public string Issuer { get; set; }
      public string Audience { get; set; }
  }
}
