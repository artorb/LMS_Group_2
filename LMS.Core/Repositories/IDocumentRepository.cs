using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        Task<bool> HasUserSubmittedAssignment(ApplicationUser user, Activity activity);
    }

}