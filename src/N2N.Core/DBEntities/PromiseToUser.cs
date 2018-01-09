using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Core.DBEntities
{
    public class PromiseToUser
    {
        public long Id { get; set; }
        /// <summary>
        /// Id of promise
        /// </summary>
        public Guid PromiseId { get; set; }

        /// <summary>
        /// UserId Whom Promiose been given
        /// </summary>
        public Guid ToUserId { get; set; }

        /// <summary>
        /// Flag to show that promise were fulfilled
        /// </summary>
        public bool IsFulfilled { get; set; }
    }
}
