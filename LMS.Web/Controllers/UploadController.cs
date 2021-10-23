using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

                                //creating document in the database
                                var document = new Document
                                {
                                    Name = $"{file.FileName}",
                                    Description = $"{activity.Description}", 
                                    UploadDate = DateTime.Now,
                                    HashName = $"{userLoggedIn.Id.ToString()}/{activity.Module.Course.Name}/{activity.Module.Name}/{activity.Name}/{file.FileName}",
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
                return RedirectToAction("Index", "Students");
            }
            
            //teacher upload into course / into module / into activity
            if (User.IsInRole("Teacher"))
            {
                //get userIdfor teacher (CourseId == NULL!)
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userLoggedIn = _unitOfWork.UserRepository.FindAsync(userId).Result;

                //get clicked activity (contains ModuleId) 
                var clickedActivity = _unitOfWork.ActivityRepository.FindAsync(activityId).Result;
                var moduleId = clickedActivity.ModuleId;

                //get module for the activity (module contains courseId)
                var moduleToClickedActivity = _unitOfWork.ModuleRepository.FindAsync(moduleId).Result;
                var courseId = moduleToClickedActivity.CourseId;

                //get course for the activity 
                var courseToClickedActivity = _unitOfWork.CourseRepository.FindAsync(courseId).Result;

                var paths = new List<string>();
                foreach (var file in files)
                {
                    if (file == null) continue;
                    if (!Directory.Exists($"wwwroot/Uploads/{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}/{clickedActivity.Name}"))
                {
                    //creating folders for teacher (course/module/activity)
                    Directory.CreateDirectory($"wwwroot/Uploads/{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}/{clickedActivity.Name}");
                }


      
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    $"wwwroot/Uploads/{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}/{clickedActivity.Name}", file.FileName);
    
                paths.Add(filePath);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                    //creating document in the database
                    var document = new Document
                    {
                        Name = $"{file.FileName}",
                        Description = $"{clickedActivity.Description}",
                        UploadDate = DateTime.Now,
                        HashName = $"{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}/{clickedActivity.Name}/{file.FileName}",
                        Uploader = $"{userLoggedIn.Email}",
                        //ApplicationUser = "",
                        Course = courseToClickedActivity,
                        Module = moduleToClickedActivity,
                        Activity = clickedActivity
                    };
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();

                    //----------------------------------------------------------------------------------------------------------------------
                  
                            }
                return RedirectToAction("Index", "Teachers");
            }                   
            
            return RedirectToAction("Index", "Students");
        }


        [HttpPost]
        public async Task<ActionResult> TeacherUploadFilesForModule(List<IFormFile> files, int moduleId)
        {
            var size = files.Sum(f => f.Length);
            // update Documents for User
     
            //teacher upload into course / into module / into activity
     
                //get userIdfor teacher (CourseId == NULL!)
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userLoggedIn = _unitOfWork.UserRepository.FindAsync(userId).Result;           

                //get clicked module (module contains courseId)
                var moduleToClickedActivity = _unitOfWork.ModuleRepository.FindAsync(moduleId).Result;
                var courseId = moduleToClickedActivity.CourseId;              
                var courseToClickedActivity = _unitOfWork.CourseRepository.FindAsync(courseId).Result;

                var paths = new List<string>();
                foreach (var file in files)
                {
                    if (file == null) continue;
                    if (!Directory.Exists($"wwwroot/Uploads/{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}"))
                    {
                        //creating folders for teacher (course/module/activity)
                        Directory.CreateDirectory($"wwwroot/Uploads/{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}");
                    }


                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        $"wwwroot/Uploads/{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}", file.FileName);

                    paths.Add(filePath);

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    //creating document in the database
                    var document = new Document
                    {
                        Name = $"{file.FileName}",
                        Description = $"{moduleToClickedActivity.Description}",
                        UploadDate = DateTime.Now,
                        HashName = $"{courseToClickedActivity.Name}/{moduleToClickedActivity.Name}/{file.FileName}",
                        Uploader = $"{userLoggedIn.Email}",                    
                        ModuleId = moduleId,
                        Course = courseToClickedActivity
                    };
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();    

                }
                return RedirectToAction("Index", "Teachers");   
        }



    }
}