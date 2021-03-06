using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Lms.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LmsDbContext _context;

        public UserRepository(LmsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<ApplicationUser> FirstOrDefaultAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        

        public async Task<ApplicationUser> GetIncludeTest(string id, Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>> includes = null)
        {
            IQueryable<ApplicationUser> queryable = _context.Set<ApplicationUser>();

            if (includes != null)
            {
                queryable = includes(queryable);
            }

            return await queryable.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<bool> AnyAsync(string? id)
        {
            return await _context.Users.AnyAsync(g => g.Id.Equals(id));
        }


        public async Task<ApplicationUser> FindAsync(string? id)
        {
            return await _context.Users.FindAsync(id);
        }


        public void Update(ApplicationUser user)
        {
            _context.Users.Update(user);
        }


        public void Remove(ApplicationUser user)
        {
            _context.Users.Remove(user);
        }
    }
}