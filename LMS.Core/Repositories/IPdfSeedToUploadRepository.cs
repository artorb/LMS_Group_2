using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.Core.Models;

namespace Lms.Core.Repositories
{
   public interface IPdfSeedToUploadRepository
    {
        List<PdfSeedToUpload> GetpdfSeedToUpload();
    }
}
