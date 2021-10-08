using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;

namespace Lms.Data.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(LmsDbContext context):base(context)
        {

        }
    }
}