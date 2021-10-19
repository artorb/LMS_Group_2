using Lms.Core.Models;
using Lms.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class PdfSeedToUploadRepository : IPdfSeedToUploadRepository
    {
   
        List<PdfSeedToUpload> IPdfSeedToUploadRepository.GetpdfSeedToUpload()
        {
            var dataToShow = new List<PdfSeedToUpload>();
            dataToShow.Add(new PdfSeedToUpload() { Id = 1, Text = "testing it" });

            return dataToShow;
        }
    }
}
