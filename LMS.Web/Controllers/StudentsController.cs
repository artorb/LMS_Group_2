using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityService _activityService;

        public StudentsController(IUnitOfWork unitOfWork, IActivityService activityService)
        {
            _unitOfWork = unitOfWork;
            _activityService = activityService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> CourseDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId =
                UserLoggedIn.CourseId; 
            var course = await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, d => d.Documents.Where(m=>m.ApplicationUser==null));

            var model = new StudentCourseViewModel()
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CourseStartDate = course.StartDate,
                CourseEndDate = course.EndDate,
                Documents = course.Documents
            };
            return PartialView("_CourseDetailsPartial", model);
        }

        // public async Task<IActionResult> ModuleDetails()
        // {
        //     var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //
        //     var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
        //     var courseId =
        //         UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts
        //     var course = await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, d => d.Documents);
        //     return null;
        // }


        public async Task<IActionResult> CourseStudentsDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId; 
            var course = await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, d => d.Users);

            var models = (from user in course.Users
                where user.Id != userId
                select new StudentCommonCourseViewModel { StudentName = user.Name, Email = user.Email }).ToList();
            return PartialView("_CourseStudentsPartial", models);
        }


        public async Task<IActionResult> ModuleList()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn =
                await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId =
                UserLoggedIn.CourseId; 

            var course = _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, m => m.Modules).Result;
            var modulesToCourse = course.Modules;

            return PartialView("GetModuleListPartial", modulesToCourse);
        }


        // kolla generic repo för att undvika fler anrop - inte nödvändigt
        public IActionResult ModuleDetail(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var module = _unitOfWork.ModuleRepository
                .GetWithIncludesIdAsync((int)Id, d => d.Documents, a => a.Activities)
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
                Status = _activityService.GetStatusForStudentModule(module, userId).Result
            };

            return PartialView("GetModuleDetailsPartial", model);
        }

        public IActionResult ActivityDetail(int Id)
        {
            var activity = _unitOfWork.ActivityRepository
                .GetWithIncludesIdAsync((int)Id, d => d.Documents, a => a.ActivityType).Result;
            if (activity == null) return NotFound();
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = new StudentActivityViewModel()
            {
                Id = activity.Id,
                ActivityName = activity.Name,
                ActivityTypes = activity.ActivityType,
                ActivityDescription = activity.Description,
                ActivityStartDate = activity.StartDate,
                ActivityEndDate = activity.EndDate,
                Documents = activity.Documents,
                Status = _activityService.GetStatusForStudentActivity(activity, userId).Result
            };

            return PartialView("GetActivityDetailsPartial", model);
        }
    }
}