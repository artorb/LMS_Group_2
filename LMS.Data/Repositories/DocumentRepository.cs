using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
   public class DocumentRepository: GenericRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(LmsDbContext context) : base(context)
        {

        }
    }
}
