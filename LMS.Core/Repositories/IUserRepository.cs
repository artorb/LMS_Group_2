using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Lms.Core.Repositories
{
    public interface IUserRepository
    {

        Task<IEnumerable<ApplicationUser>> GetAllAsync();

        Task<ApplicationUser> FirstOrDefaultAsync(string id);

        Task<ApplicationUser> GetIncludeTest(string id, Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>> includes = null);

        Task<bool> AnyAsync(string? id);

        Task<ApplicationUser> FindAsync(string? id);

        void Update(ApplicationUser user);

        void Remove(ApplicationUser user);
    }
}