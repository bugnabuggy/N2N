﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Infrastructure.Models.DTO
{
    public class LoginResponseDTO
    {
        public string access_token { get; set; }
        public DateTime expiration_date { get; set; }

        public string refresh_token { get; set; }
        
    }
}