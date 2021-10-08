using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.Core.Repositories;
using Lms.Data.Data;

namespace Lms.Data.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly LmsDbContext db;
        public IActivityRepository ActivityRepository { get; }
        public ICourseRepository CourseRepository { get; }

        public UnitOfWork(LmsDbContext context)
        {
            db = context;
            ActivityRepository = new ActivityRepository(db);
            CourseRepository = new CourseRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
