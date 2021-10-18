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
        Task<ApplicationUser> FirstOrDefaultAsync(string id);

        Task<ApplicationUser> GetIncludeTest(string id,
            Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>> includes = null);
    }
}
