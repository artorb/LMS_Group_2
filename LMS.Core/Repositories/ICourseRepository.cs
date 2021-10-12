using Lms.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface ICourseRepository:IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> FindByNameAsync(string courseName);
    }
}