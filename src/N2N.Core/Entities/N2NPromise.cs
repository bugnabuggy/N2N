﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Core.Entities
{
    public class N2NPromise
    {
        public Guid Id { get; set; }
        public string Text{ get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPublic { get; set; }
        public string BlockChainTransaction { get; set; }

        public Guid N2NUserId { get; set; }
    }
}
