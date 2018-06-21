using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;

namespace N2N.Core.Models
{
    public class UserStatistics
    {
        public N2NUser N2NUser { get; set; }
        public int PromisesCount { get; set; }
        public int PostcardsCount { get; set; }
        public int GiftsCount { get; set; }
        public int ToUserPromisesCount { get; set; }
    }
}
