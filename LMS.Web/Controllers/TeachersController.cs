using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly LmsDbContext db;

        public TeachersController(IUnitOfWork unitOfWork, LmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            db = context;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(TeacherCreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activity = new Activity
                {
                    Name = model.ActivityName,
                    Description = model.ActivityDescription,
                    StartDate = model.ActivityStartDate,
                    EndDate = model.ActivityEndDate,
                    Deadline = model.ActivityDeadline,
                    ActivityType = new ActivityType
                    {
                        TypeName = "Laboratory"
                    }
                    //ActivityType = (ActivityType)await GetAllActivityTypesAsync()
                };
                
                var module = new Module
                {
                    Name = model.ModuleName,
                    Description = model.ModuleDescription,
                    StartDate = model.ModuleStartDate,
                    EndDate = model.ModuleEndDate,
                    Activities = new List<Activity> { activity }
                    // Activities = new List<Activity>()
                };

                var course = new Course
                {
                    Name = model.CourseName,
                    Description = model.CourseDescription,
                    StartDate = model.CourseStartDate,
                    EndDate = model.CourseEndDate,
                    Modules = new List<Module> { module }
                };

                // activity.ActivityType = new ActivityType
                // {
                //     TypeName = "Laboratory"
                // };
                //
                // activity.Module = module;
                // module.Course = course;

                // _unitOfWork.ModuleRepository.Add(module);
                // _unitOfWork.ActivityRepository.Add(activity);
                _unitOfWork.CourseRepository.Add(course);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index()
        {
            /*
                ViewData["ActivityTypeId"] = new SelectList(_unitOfWork.ActivityTypes, "Id", "Id");
                ViewData["ModuleId"] = new SelectList(_unitOfWork.Modules, "Id", "Id");
             * Test with ViewData["CourseId"] instead of StudentAndTeacherViewModel
             */
            var courses = await _unitOfWork.CourseRepository.GetAllWithIncludesAsync(m => m.Modules, m => m.Users);

            var teacherTable = courses.Select(course => new TeacherTableViewModel
                {
                    Course = course,
                    ActiveModuleName =
                        course.Modules.ElementAt(0).Name, //Fix to use StartDate and EndDate, and add null-handler
                    NextModuleName =
                        course.Modules.ElementAt(1).Name, //Fix to use StartDate and EndDate, and add null-handler
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
        // public async Task<IActionResult> Search(string courseName)
        // {
        //     var foundCourses = await _unitOfWork.CourseRepository.FindByNameAsync(courseName);
        //
        //     var viewModels = new List<TeacherTableViewModel>();
        //
        //     foreach (var course in foundCourses)
        //     {
        //         var viewModel = new TeacherTableViewModel()
        //         {
        //             Course = course,
        //             ActiveModuleName = course.Modules.ElementAt(0).Name,
        //             NextModuleName = course.Modules.ElementAt(1).Name,
        //             NumberOfParticipants = course.Users.Count
        //         };
        //         viewModels.Add(viewModel);
        //     }
        //
        //     return View(nameof(Index), viewModels);
        // }


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

            //if student uploaded something : problem with documents. ApplicationUserId should be NULL
            //var documentsForUserWillBeDeleted = _unitOfWork.DocumentRepository.GetAllAsync().Result.Where(u=>u.ApplicationUserId == id);


            //foreach (var item in documentsForUserWillBeDeleted)
            //{
            //    item.ApplicationUserId = null;
            //}

            TempData["DeleteTeacher"] = $"The {userWillBeDeleted.Name} has been deleted!";
            _unitOfWork.UserRepository.Remove(userWillBeDeleted);
            await _unitOfWork.CompleteAsync();  
            return RedirectToAction("PersonnalList", "Home");
        }
    }
}