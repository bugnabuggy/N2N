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
using N2N.Infrastructure.Repositories;
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
            IQueryable<UserStatistics> result = null;

            result = _context.N2NUsers
                    .GroupJoin(
                        _context.Promises,
                        u => u.Id,
                        p => p.N2NUserId,
                        (u, p) => new UserStatistics()
                        {
                            N2NUser = u,
                            PromisesCount = p.Count()
                        })
                    .GroupJoin(
                        _context.PromisesToUsers,
                        stat => stat.N2NUser.Id,
                        ptu => ptu.ToUserId,
                        (stat, ptu) => new UserStatistics()
                        {
                            N2NUser = stat.N2NUser,
                            PromisesCount = stat.PromisesCount,
                            ToUserPromisesCount = ptu.Count()
                        })
                    .GroupJoin(
                        _context.Postcards,
                        stat => stat.N2NUser.Id,
                        pc => pc.N2NUserId,
                        (stat, pc) => new UserStatistics()
                        {
                            N2NUser = stat.N2NUser,
                            PromisesCount = stat.PromisesCount,
                            ToUserPromisesCount = stat.ToUserPromisesCount,
                            PostcardsCount = pc.Count()
                        });

            if (filter != null)
            {
                result = result.Where(filter);
            }

            if (orderBy != null)
            {
                result = orderBy(result);
            }

            return result.AsEnumerable();
        }
    }
}
