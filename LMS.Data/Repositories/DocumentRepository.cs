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
   public class DocumentRepository: GenericRepository<Document>, IDocumentRepository
    {
        private readonly LmsDbContext _context;
        public DocumentRepository(LmsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasUserSubmittedAssignment(ApplicationUser user, Activity activity)
        {
            return await _context.Documents
                 .Where(a => a.ActivityId == activity.Id)
                 .AnyAsync(a => a.Uploader == user.Email);
        }
    }
}
