using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Lms.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Lms.Core.Models.ViewModels;
using Lms.Web.Service;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LmsDbContext _context;
        private readonly IActivityService _activityService;

        public StudentsController(LmsDbContext context, IUnitOfWork unitOfWork, IActivityService activityService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _activityService = activityService;
        }

        public async Task<IActionResult> Index()
        {
            var schema = await VeckoSchema();
            return View(schema);
        }

        public async Task<IActionResult> CourseDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId =
                UserLoggedIn.CourseId;
            var course = await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, d => d.Documents.Where(m => m.ApplicationUser == null));

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

        public async Task<List<DaySchemaViewModel>> VeckoSchema()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId;

            var modules = await _context.Modules
                .Include(m => m.Activities).ThenInclude(a => a.ActivityType)
                .Where(m => m.CourseId == courseId && m.EndDate > DateTime.Today && m.StartDate <= DateTime.Today.AddDays(7))
                .OrderBy(m => m.StartDate).ToListAsync();

            List<Activity> activities = new();
            foreach (var module in modules)
            {
                var act = module.Activities
                    .Where(a => a.EndDate > DateTime.Today && a.StartDate <= DateTime.Today.AddDays(7))
                    .OrderBy(a => a.StartDate).ToList();
                activities.AddRange(act);
            }
            activities = activities.OrderBy(a => a.StartDate).ToList();
            List<DaySchemaViewModel> weekSchema = new();

            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.Today.AddDays(i);

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;

                var daySchema = new DaySchemaViewModel()
                {
                    WeekDay = date.DayOfWeek.ToString(),
                    ActivitySchemas = new List<ActivitySchemaViewModel>()
                };

                //Test för Dealine hantering
                //if (date.DayOfWeek.ToString()=="Friday")
                //{
                //    var activitySchema = new ActivitySchemaViewModel()
                //    {
                //        Name = "TeST",
                //        ActivityTypeName = "Lecture",
                //        DeadLine = true
                //    };

                //    daySchema.ActivitySchemas.Add(activitySchema);
                //}

                foreach (var activity in activities)
                {
                    if (activity.StartDate <= date && activity.EndDate >= date)
                    {
                        var activitySchema = new ActivitySchemaViewModel()
                        {
                            Name = activity.Name,
                            ActivityTypeName = activity.ActivityType.TypeName,
                            DeadLine = activity.Deadline == date ? true : false
                        };

                        daySchema.ActivitySchemas.Add(activitySchema);
                    }
                }
                weekSchema.Add(daySchema);
            }

            return weekSchema;
        }
    }
}