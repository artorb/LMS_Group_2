using System.Threading.Tasks;
using Lms.Core.Entities;

namespace Lms.Core.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FirstOrDefaultAsync(string id);
    }
}