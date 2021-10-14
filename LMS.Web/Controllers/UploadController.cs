using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Controllers
{
    public class UploadController : Controller
    {
       [HttpPost]  
       public async Task<ActionResult> UploadFiles(List<IFormFile> files)
       {
           var size = files.Sum(f => f.Length);

           var paths = new List<string>();
           foreach (var file in files)
           {
               if (file == null) continue;

               var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", file.FileName);
               // var filePath = Path.Combine("~/Uploads", file.FileName);
               paths.Add(filePath);

               await using (var stream = new FileStream(filePath, FileMode.Create))
               {
                   await file.CopyToAsync(stream);
               }
               ViewBag.UploadStatus = files.Count + " files uploaded successfully.";
           }  
      
           return Ok(new { Count = files.Count, size, paths});  
       }
    }
}