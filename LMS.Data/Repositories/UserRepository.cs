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
    }
}