using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly LmsDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LmsDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public TeachersController(IUnitOfWork unitOfWork, LmsDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            db = context;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }


        private async Task<IEnumerable<SelectListItem>> GetAllActivityTypesAsync()
        {
            return await db.ActivityTypes.Select(act => new SelectListItem
            {
                Text = act.TypeName,
                Value = act.Id.ToString(),
            }).ToListAsync();
        }

        private Activity _activity { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Activity> CreateActivity(CreateActivityViewModel activityModel)
        {
            if (ModelState.IsValid)
            {
                var activity = new Activity
                {
                    Name = activityModel.ActivityName,
                    Description = activityModel.ActivityDescription,
                    StartDate = activityModel.ActivityStartDate,
                    EndDate = activityModel.ActivityEndDate,
                    Deadline = activityModel.ActivityDeadline,
                    ActivityType = new ActivityType
                    {
                        TypeName = "Laboratory"
                    }
                    //ActivityType = (ActivityType)await GetAllActivityTypesAsync()
                };
                _activity = activity;
                return activity;
            }
            return null;
        }

        private Module _module { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Module> CreateModule(CreateModuleViewModel moduleModel)
        {
            if (ModelState.IsValid)
            {
                var module = new Module
                {
                    Name = moduleModel.ModuleName,
                    Description = moduleModel.ModuleDescription,
                    StartDate = moduleModel.ModuleStartDate,
                    EndDate = moduleModel.ModuleEndDate,
                    Activities = new List<Activity> { _activity }
                    //Activities = new List<Activity>()
                };
                _module = module;
                return module;
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(TeacherCreateCourseViewModel courseModel)
        {
            if (ModelState.IsValid)
            {
                var course = new Course
                {
                    Name = courseModel.CourseName,
                    Description = courseModel.CourseDescription,
                    StartDate = courseModel.CourseStartDate,
                    EndDate = courseModel.CourseEndDate,
                    Modules = new List<Module> { _module }
                };


                _unitOfWork.CourseRepository.Add(course);
                await _unitOfWork.CompleteAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Index()
        {

            var courses = await _unitOfWork.CourseRepository.GetAllWithIncludesAsync(m => m.Modules, m => m.Users);

            var teacherTable = courses.Select(course => new TeacherTableViewModel
            {
                Course = course,
                ActiveModuleName = GetCurrentModules(course)[0],

                NextModuleName = GetCurrentModules(course)[1],
                        
                NumberOfParticipants = course.Users.Count
            })
                .ToList();

            var indexVM = new TeacherIndexViewModel
            {
                CourseToCreate = new TeacherCreateCourseViewModel(),
                TeacherTables = teacherTable
            };

            // return View(viewModels);
            return View(indexVM);
        }

        private static List<string> GetCurrentModules(Course course)
        {
            var activeModule = course.Modules
                .Where(m => m.StartDate < DateTime.Today && m.EndDate >= DateTime.Today)
                .FirstOrDefault();
            var nextModule = course.Modules
                .Where(m => m.StartDate.Date >= activeModule.EndDate.Date)
                .FirstOrDefault();

            var activeModuleName = activeModule != null ? activeModule.Name : "No more Modules";
            var nextModuleName = nextModule != null ? nextModule.Name : "No more Modules";

            List<string> result = new() { activeModuleName, nextModuleName };

            return result;
        }


        public async Task<IActionResult> IndexCourseForTeacher(int id)
        {
            ViewData["CourseId"] = id;
            // var model = new StudentsAndTeachersIndexViewModel()
            // {
            //     idFromCourse = id
            // };
            // return View(model);
            return View();
        }

        public PartialViewResult GetSomeData()
        {
            ViewBag.Message = "Example Data from Server"; //Using ViewBag Just for example, use ViewModel Instead
            return PartialView("~/Views/Teachers/CreateCourse.cshtml");
        }
    }
}