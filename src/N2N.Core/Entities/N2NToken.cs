﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Entities
{
    public class N2NToken
    {
        public Guid Id { get; set; }
        public Guid N2NUserId { get; set; }
        public DateTime TokenExpirationDate { get; set; }
        public Guid IdRefreshToken { get; set; }

    }
}
