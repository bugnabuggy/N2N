﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Interfaces;

namespace N2N.Core.Entities
{
    public class UserPromise : IOwned
    {
        public Guid Id { get; set; }
        public Guid PromiseId { get; set; }
        public Guid N2NUserId { get; set; }
    }
}