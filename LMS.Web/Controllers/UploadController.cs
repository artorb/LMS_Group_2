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
        public async Task<ActionResult> UploadFiles(List<IFormFile> files, string clickedActivityName)    
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


                /*
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
                */

                var paths = new List<string>();
                foreach (var file in files)
                {
                    if (file == null) continue;
                    foreach (var module in userLoggedIn.Course.Modules)
                    {
                        foreach (var activity in module.Activities)
                        {
                            //creating folders for student (course/module/activity)
                            Directory.CreateDirectory($"wwwroot/Uploads/{userLoggedIn.Id.ToString()}/" +
                                                      $"{userLoggedIn.Course.Name}/" +
                                                      $"{module.Name}/" +
                                                      $"{activity.Name}");

                            //if (activity.Name == clickedActivityName) //.... this should be from the GetActivityDetailsPartial View) **************
                            //{

                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Uploads/{userLoggedIn.Id.ToString()}/" +
                                                      $"{userLoggedIn.Course.Name}/" +
                                                      $"{module.Name}/" +
                                                      $"{activity.Name}", file.FileName);                               
                                paths.Add(filePath);

                                await using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }


                            var document =  new Document
                            {
                                Name = $"{file.FileName}",
                                Description = $"{activity.Description}", //what is the document description?
                                UploadDate = System.DateTime.Now,
                                HashName = $"{activity.Module.Course.Name}/{activity.Module.Name}/{activity.Name}/{file.FileName}",
                                Uploader = $"{userLoggedIn.Email}",
                                Activity = activity
                            };
                            _context.Documents.Add(document);
                            await _context.SaveChangesAsync();

                            }
                        //}
                    }
                }










                    }



            /*
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
            */




            return RedirectToAction("Index", "Students");
            //return Ok(new { Count = files.Count, size, paths });        
       }
    }
}