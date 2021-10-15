using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Controllers
{
    public class DownloadController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            //filePath = "Picture.jpg"; // FIXME (static file)
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", filePath);
            var mem = new MemoryStream();

            await using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(mem);
            }

            mem.Position = 0;
            const string contentType = "Application/octet-stream";
            var fileName = Path.GetFileName(path);
            return File(mem, contentType, fileName);
        }
    }
}