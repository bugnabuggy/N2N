using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using N2N.Infrastructure.DataContext;

namespace N2N.Data.Repositories
{
    public class DbRepository<T> : IRepository<T> where T : class
    {
        private N2NDataContext _ctx;
        private DbSet<T> _table;
        public IQueryable<T> Data { get; }

        public DbRepository(N2NDataContext ctx)
        {
            _ctx = ctx;
            _table = _ctx.Set<T>();
            Data = _table;
        }

        public T Update(T entity)
        {
            _table.Add(entity);
            _ctx.SaveChanges();
            return entity;
        }

        public T Add(T entity)
        {
            _table.Add(entity);
            _ctx.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            _table.Remove(entity);
            _ctx.SaveChanges();
            return entity;
        }


        public IEnumerable<T> Add(IEnumerable<T> items)
        {
            _table.AddRange(items);
            _ctx.SaveChanges();
            return items;
        }

        public IEnumerable<T> Update(IEnumerable<T> items)
        {
            _table.UpdateRange(items);
            _ctx.SaveChanges();
            return items;
        }

        public IEnumerable<T> Delete(IEnumerable<T> items)
        {
            _table.RemoveRange(items);
            _ctx.SaveChanges();
            return items;
        }
    }
}
