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
        public IActivityTypeRepository ActivityTypeRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public IUserRepository UserRepository { get;  }
        public IDocumentRepository DocumentRepository { get; }

        public UnitOfWork(LmsDbContext context)
        {
            db = context;
            ActivityRepository = new ActivityRepository(db);
            ActivityTypeRepository = new ActivityTypeRepository(db);
            CourseRepository = new CourseRepository(db);
            ModuleRepository = new ModuleRepository(db);
            UserRepository = new UserRepository(db);
            DocumentRepository = new DocumentRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
