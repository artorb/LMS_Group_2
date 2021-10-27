using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        private readonly LmsDbContext _context;
        public ModuleRepository(LmsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Activity>> GetSortedListOfWeeklyActivitiesAsync(int courseId)
        {
            List<Activity> activities = new();

            await _context.Modules
               .Include(m => m.Activities).ThenInclude(a => a.ActivityType)
               .Where(m => m.CourseId == courseId && m.EndDate > DateTime.Today && m.StartDate <= DateTime.Today.AddDays(7))
               .ForEachAsync(m => activities.AddRange(m.Activities.Where(a => a.EndDate > DateTime.Today && a.StartDate <= DateTime.Today.AddDays(7)).ToList()));

            return activities.OrderBy(a => a.StartDate).ToList();
        }
    }
}
