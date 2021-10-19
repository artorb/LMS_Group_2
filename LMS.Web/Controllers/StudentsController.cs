using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Lms.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;
using Lms.Core.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly LmsDbContext _context;
        private readonly IUnitOfWork _unitOfWork;


        public StudentsController(LmsDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> CourseDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); //Uses _context, need to change
            var courseId =
                UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts
            var course = await _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d => d.Documents.Where(x=>x.ApplicationUserId == null));
                

            var model = new StudentCourseViewModel()
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CourseStartDate = course.StartDate,
                CourseEndDate = course.EndDate,
                Documents = course.Documents //where ApplicationUserId == null
            };
            return PartialView("_CourseDetailsPartial", model);
        }


        // IS IT USED SOMEWHERE?
        public async Task<IActionResult> ModuleDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); //Uses _context, need to change
            var courseId =
                UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts
            var course = await _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d => d.Documents);

            // var model = new StudentModuleViewModel()
            // {
            //     Information = "",
            //     StartDate = null,
            //     EndDate = null,
            //     Activity = null,
            //     Documents = null,
            // };
            // return PartialView("_CourseModulePartial", model);
            return null;
        }


        public async Task<IActionResult> CourseStudentsDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn =
                // await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); //Uses _context, need to change
                await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId =
                UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts
            var course = await _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d => d.Users);

            var models = (from user in course.Users
                where user.Id != userId
                select new StudentCommonCourseViewModel { StudentName = user.Name, Email = user.Email }).ToList();
            return PartialView("_CourseStudentsPartial", models);
        }


        public async Task<IActionResult> ModuleList()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn =
                //_context.Users.FirstOrDefault(u => u.Id == userId);//Uses _context, need to change  
                await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId =
                UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts   

            var course = _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, m => m.Modules).Result;
            var modulesToCourse = course.Modules;

            return PartialView("GetModuleListPartial", modulesToCourse);
        }


        // kolla generic repo för att undvika fler anrop - inte nödvändigt
        public IActionResult ModuleDetail(int Id)
        {    
            var module = _unitOfWork.ModuleRepository.GetWithIncludesAsync((int)Id, d => d.Documents.Where(x => x.ApplicationUserId == null), a => a.Activities)
                .Result;
            if (module == null) return NotFound();

            if (module.Activities != null)
            {
                foreach (var item in module.Activities)
                {
                    item.ActivityType = _unitOfWork.ActivityTypeRepository.GetAsync(item.ActivityTypeId).Result;
                }
            }

            var model = new StudentModuleViewModel()
            {
                ModuleName = module.Name,
                ModuleDescription = module.Description,
                ModuleStartDate = module.StartDate,
                ModuleEndDate = module.EndDate,
                Documents = module.Documents,
                Activities = module.Activities,
                Status = GetStatusForStudentModule(module).Result
            };

            return PartialView("GetModuleDetailsPartial", model);
        }

        public IActionResult ActivityDetail(int Id)
        {
            var activity = _unitOfWork.ActivityRepository
                .GetWithIncludesAsync((int)Id, d => d.Documents, a => a.ActivityType).Result;
            if (activity == null) return NotFound();

            var model = new StudentActivityViewModel()
            {
                Id = activity.Id,
                ActivityName = activity.Name,
                ActivityTypes = activity.ActivityType,
                ActivityDescription = activity.Description,
                ActivityStartDate = activity.StartDate,
                ActivityEndDate = activity.EndDate,
                Documents = activity.Documents,
                Status = GetStatusForStudentActivity(activity).Result
            };

            return PartialView("GetActivityDetailsPartial", model);
        }

        private async Task<string> GetStatusForStudentActivity(Activity clickedActivity)
        {
          
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var documents = await _unitOfWork.DocumentRepository.GetIncludeTest(a => a.Include(x=>x.Activity));     
         
            var documentForActivity = documents.FirstOrDefault(a => a.Uploader == UserLoggedIn.Email);
            if (documentForActivity == null)
                return "Not uploaded";
            if (documentForActivity.Activity.Name== clickedActivity.Name) {           
                if (documentForActivity.UploadDate > clickedActivity.Deadline)
                    return "Delayed";
                if (documentForActivity.UploadDate < clickedActivity.Deadline)
                    return "Uploaded";
            }
            return "Not uploaded";
        }


        private async Task<string> GetStatusForStudentModule(Module clickedModule)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var documents = await _unitOfWork.DocumentRepository.GetIncludeTest(a => a.Include(x => x.Activity).ThenInclude(y=>y.Module));

            var documentForActivity = documents.FirstOrDefault(a => a.Uploader == UserLoggedIn.Email);
            if (documentForActivity == null)
                return "Not uploaded";
            if (documentForActivity.Module.Name == clickedModule.Name)
            {
                if (documentForActivity.UploadDate > clickedModule.EndDate)
                    return "Delayed";
                if (documentForActivity.UploadDate < clickedModule.EndDate)
                    return "Uploaded";
            }
            return "Not uploaded";
        }


    }
}