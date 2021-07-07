using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Models;

namespace N2N.Core.Services
{
    public interface IUsersStatisticsService
    {
        IEnumerable<UserStatistics> GetUsersStatistics( Expression<Func<UserStatistics, bool>> filter, Func<IQueryable<UserStatistics>,
                                                        IOrderedQueryable<UserStatistics>> orderBy
            );
    }
}
