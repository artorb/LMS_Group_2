﻿using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetWithIncludesAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindAsync(int? id);
        Task<bool> AnyAsync(int? id);
        Task<T> GetAsync(int id);
        void Add(T obj);
        void Update(T obj);
        void Remove(T obj);
    }
}
