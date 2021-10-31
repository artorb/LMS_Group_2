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
using System.Security.Claims;
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
                .FirstOrDefault(m => m.StartDate < DateTime.Today && m.EndDate >= DateTime.Today);
            Module nextModule = new();
            if (activeModule != null)
            {
                nextModule = course.Modules
                    .FirstOrDefault(m => m.StartDate.Date >= activeModule.EndDate.Date);
            }
            else
            {
                nextModule = null;
            }

            var activeModuleName = activeModule != null ? activeModule.Name : "No more Modules";
            var nextModuleName = nextModule != null ? nextModule.Name : "No more Modules";

            List<string> result = new() { activeModuleName, nextModuleName };

            return result;
        }


        public async Task<IActionResult> IndexCourseForTeacher(int id)
        {
            ViewData["CourseId"] = id;
 
            return View();
        }

        public PartialViewResult GetSomeData()
        {
            ViewBag.Message = "Example Data from Server"; //Using ViewBag Just for example, use ViewModel Instead
            return PartialView("~/Views/Teachers/CreateCourse.cshtml");
        }

        // GET: Students/EditTeacher/5
        [HttpGet]
        public async Task<IActionResult> EditTeacher(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var UserClicked = await _unitOfWork.UserRepository.FirstOrDefaultAsync(Id);

            if (UserClicked == null)
            {
                return NotFound();
            }

            var model = new ApplicationUserViewModel
            {
                Id = UserClicked.Id,
                Name = UserClicked.Name,
                Email = UserClicked.Email
            };

            return View(model);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeacher(string Id, ApplicationUserViewModel applicationUsermodel)
        {
            if (Id != applicationUsermodel.Id)
            {
                return NotFound();
            }
            var userInDatabase = await db.Users.FirstOrDefaultAsync(u => u.Id == Id);

            if (ModelState.IsValid)
            {
                try
                {

                    userInDatabase.Email = applicationUsermodel.Email;
                    userInDatabase.NormalizedEmail = applicationUsermodel.Email.ToUpper();
                    userInDatabase.Name = applicationUsermodel.Name;
                    userInDatabase.UserName = applicationUsermodel.Name;
                    userInDatabase.NormalizedUserName = applicationUsermodel.Name.ToUpper();


                    _unitOfWork.UserRepository.Update(userInDatabase);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUsermodel.Id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PersonnalList", "Home");
            }
            return View(applicationUsermodel);
        }

        private async Task<bool> ApplicationUserExists(string Id)
        {
            return await _unitOfWork.UserRepository.AnyAsync(Id);
        }

        // GET: Teachers/DeleteTeacher/5
        public async Task<IActionResult> DeleteTeacher(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            if (UserLoggedIn.Id == id)//Checks that the user is logged in as someone from the database.
            {
                TempData["DeleteTeacher"] = $"The {UserLoggedIn.Name} can not be deleted, since they are logged in!";
                return RedirectToAction("PersonnalList", "Home");
            }

            var teacher = await _unitOfWork.UserRepository.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }



        // POST: Teachers/DeleteTeacher/5
        [HttpPost, ActionName("DeleteTeacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeacherConfirmed(string id)
        {
            var userWillBeDeleted = await _unitOfWork.UserRepository.FirstOrDefaultAsync(id);  

            TempData["DeleteTeacher"] = $"The {userWillBeDeleted.Name} has been deleted!";
            _unitOfWork.UserRepository.Remove(userWillBeDeleted);
            await _unitOfWork.CompleteAsync();  
            return RedirectToAction("PersonnalList", "Home");
        }
    }
}