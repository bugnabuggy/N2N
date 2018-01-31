using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using N2N.Core.Entities;
using N2N.Core.Models;
using N2N.Core.Services;
using N2N.Data.Repositories;
using N2N.Infrastructure.DataContext;

namespace N2N.Services
{
    public class UsersStatisticsService : IUsersStatisticsService
    {
        private readonly N2NDataContext _context;
        


        public UsersStatisticsService(N2NDataContext context)
        {
            this._context = context;
        }

        public IEnumerable<UserStatistics> GetUsersStatistics(Expression<Func<UserStatistics, bool>> filter = null, Func<IQueryable<UserStatistics>, IOrderedQueryable<UserStatistics>> orderBy = null)
        {
            IEnumerable<UserStatistics> result = null;



            return result;
        }
    }
}
