using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers
{
    public class UploadController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UploadController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(List<IFormFile> files)
        {
            var size = files.Sum(f => f.Length);

            // update Documents for User

            if (User.IsInRole("Student"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var userLoggedIn = _unitOfWork.UserRepository.GetIncludeTest(userId, c =>
                    c.Include(c => c.Course).ThenInclude(c => c.Modules).ThenInclude(c => c.Activities)).Result;

                // if (!Directory.Exists($"{userLoggedIn.Id}/{userLoggedIn.Course.Name}/{userLoggedIn.}"))
                // {
                //     // Directory.CreateDirectory($"wwwroot/Uploads/{userLoggedIn.Id}/{userLoggedIn.Course}");

                    foreach (var module in userLoggedIn.Course.Modules)
                    {
                        foreach (var activity in module.Activities)
                        {
                            Directory.CreateDirectory($"wwwroot/Uploads/{userLoggedIn.Id.ToString()}/" +
                                                      $"{userLoggedIn.Course.Name}/" +
                                                      $"{module.Name}/" +
                                                      $"{activity.Name}");
                        }
                    }
                // }
            }

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

            return Ok(new { Count = files.Count, size, paths });
        }
    }
}