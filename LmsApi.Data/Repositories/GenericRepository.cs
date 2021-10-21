using LmsApi.Core.Entities;
using LmsApi.Core.Interfaces;
using LmsApi.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LmsApi.Data.Repositories
{
    /*
     * TO-DO:
     * - A generic orderby
     * - A generic search
     * - Do alot of testing regarding the methods in different scenarios
     */
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly LmsApiDbContext db = null;
        private readonly DbSet<T> table = null;

        public GenericRepository(LmsApiDbContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
            table = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> filter = null,
                                                                 Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            var entity = table.AsQueryable();

            if (filter != null)
            {
                entity = entity.Where(filter);
            }

            if (includes != null)
            {
                entity = includes(entity);
            }

            return await entity.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            var entity = table.AsQueryable();

            if (filter != null)
            {
                entity = entity.Where(filter);
            }

            return await entity.ToListAsync();
        }

        public async Task<T> GetWithIncludeAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            var entity = table.AsQueryable();

            if (includes != null)
            {
                entity = includes(entity);
            }

            return await entity.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<T> GetAsync(int id, Expression<Func<T, bool>> filter = null)
        {
            return await table.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await table.AnyAsync(t => t.Id == id);
        }

        public void Add(T entity)
        {
            table.Add(entity);
        }

        public void Delete(T entity)
        {
            table.Remove(entity);
        }

        public void Update(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

    }
}
