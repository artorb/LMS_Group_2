using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers
{
    public class UploadController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LmsDbContext _context;

        public UploadController(LmsDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(List<IFormFile> files, int activityId)
        {
            var size = files.Sum(f => f.Length);
            // update Documents for User
            if (User.IsInRole("Student"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var userLoggedIn = _unitOfWork.UserRepository.GetIncludeTest(userId, c =>
                    c.Include(c => c.Course).ThenInclude(c => c.Modules).ThenInclude(c => c.Activities)).Result;

                var paths = new List<string>();
                foreach (var file in files)
                {
                    if (file == null) continue;
                    foreach (var module in userLoggedIn.Course.Modules)
                    {
                        foreach (var activity in module.Activities)
                        {
                            if (activity.Id == activityId)
                            {
                                if (!Directory.Exists($"wwwroot/Uploads/{userLoggedIn.Id.ToString()}/" +
                                                      $"{userLoggedIn.Course.Name}/" +
                                                      $"{module.Name}/" +
                                                      $"{activity.Name}"))
                                {
                                    //creating folders for student (course/module/activity)
                                    Directory.CreateDirectory($"wwwroot/Uploads/{userLoggedIn.Id.ToString()}/" +
                                                              $"{userLoggedIn.Course.Name}/" +
                                                              $"{module.Name}/" +
                                                              $"{activity.Name}");
                                }

                                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    $"wwwroot/Uploads/{userLoggedIn.Id.ToString()}/" +
                                    $"{userLoggedIn.Course.Name}/" +
                                    $"{module.Name}/" +
                                    $"{activity.Name}", file.FileName);
                                paths.Add(filePath);

                                await using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }


                                var document = new Document
                                {
                                    Name = $"{file.FileName}",
                                    Description = $"{activity.Description}", //what is the document description?
                                    UploadDate = System.DateTime.Now,
                                    HashName =
                                        $"{activity.Module.Course.Name}/{activity.Module.Name}/{activity.Name}/{file.FileName}",
                                    Uploader = $"{userLoggedIn.Email}",
                                    ApplicationUser = userLoggedIn,
                                    Course = userLoggedIn.Course,
                                    Module = activity.Module,
                                    Activity = activity
                                };
                                _context.Documents.Add(document);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            return RedirectToAction("ActivityDetail", "Students", new { id = activityId });
            // return PartialView("ActivityDetail", "Students", new { id = activityId });
            //return Ok(new { Count = files.Count, size, paths });        
        }
    }
}