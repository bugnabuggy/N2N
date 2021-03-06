﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace N2N.Data.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Data { get; }

        T Update(T item);
        T Add(T item);
        T Delete(T item);

        IEnumerable<T> Add(IEnumerable<T> items);
        IEnumerable<T> Update(IEnumerable<T> items);
        IEnumerable<T> Delete(IEnumerable<T> items);
    }
}
