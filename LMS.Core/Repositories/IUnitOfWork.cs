using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IUnitOfWork
    {
        IActivityRepository ActivityRepository { get; }
        IActivityTypeRepository ActivityTypeRepository { get; }
        ICourseRepository CourseRepository { get; }
        IModuleRepository ModuleRepository { get; }
        //IGenericRepository<Activity> ActivityRepository { get; }
        //IGenericRepository<Activity> ActivityRepository { get; }

        Task CompleteAsync();
    }
}
