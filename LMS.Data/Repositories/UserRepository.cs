using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}