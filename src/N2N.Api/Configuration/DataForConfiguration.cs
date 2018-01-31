﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace N2N.Api.Configuration
{
    public class DataForConfiguration
    {
        public DateTime TimeLife { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}