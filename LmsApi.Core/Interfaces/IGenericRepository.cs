﻿using LmsApi.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LmsApi.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> filter = null, 
                                                    Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<T> GetAsync(int id, Expression<Func<T, bool>> filter = null);
        Task<T> GetWithIncludeAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<T> FindAsync(int id);
        Task<bool> ExistsAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
