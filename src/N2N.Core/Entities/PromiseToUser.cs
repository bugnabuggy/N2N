using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace N2N.Core.Entities
{
    public class PromiseToUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
