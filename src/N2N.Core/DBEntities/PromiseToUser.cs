using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("PromiseId")]
        public N2NPromise Promise { get; set; }


        /// <summary>
        /// UserId Whom Promiose been given
        /// </summary>
        public Guid ToUserId { get; set; }
        [ForeignKey("ToUserId")]
        public N2NUser ToUser{ get; set; }

        /// <summary>
        /// Flag to show that promise were fulfilled
        /// </summary>
        public bool IsFulfilled { get; set; }

        /// <summary>
        /// Date when promise was marked as fullfield
        /// </summary>
        public DateTime FulfillDate { get; set; }
    }
}
