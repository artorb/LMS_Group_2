using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly LmsDbContext db = null;

        public CourseRepository(LmsDbContext context):base(context)
        {
            db = context ?? throw new ArgumentNullException(nameof(db));     
        }

        public async Task<IEnumerable<Course>> FindByNameAsync(string courseName) 
        {
            return await db.Courses.Include(m=>m.Modules).Include(u=>u.Users).Where(
                string.IsNullOrWhiteSpace(courseName)? 
                c=> true :
                c => c.Name.ToUpper().Contains(courseName.ToUpper())).ToListAsync();
                //p => p.Name.Contains(courseName, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        }
    }
}