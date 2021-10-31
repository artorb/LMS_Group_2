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
//using Lms.Core.Repositories;
//using Lms.Data.Data;
using Lms.Web.Service;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var schema = await WeekSchema();       
            return PartialView("WeekSchema",schema);
        }



        public async Task<IActionResult> CourseStudentsDetails(int? idFromCourse)
        {
         

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId;

            var course = (idFromCourse == null) ?
            //Student
            await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, d => d.Users) :
            //Teacher
            await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)idFromCourse, d => d.Users);

            var models = (from user in course.Users
                          select new StudentCommonCourseViewModel
                          {
                              Id = user.Id,
                              StudentName = user.Name,
                              Email = user.Email
                          }).ToList();

            return PartialView("_CourseStudentsPartial", models);         
        }



        // GET: Students/Details/5
        public async Task<IActionResult> Details(string? Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            ViewData["Id"] = userId;

            var model = new ApplicationUserViewModel()
            {
                Name = UserLoggedIn.Name,
                Email = UserLoggedIn.Email,
                CourseId = UserLoggedIn.CourseId
            };
            return View("Details", model);
        }



        // GET: Students/Edit/5
        //string id==null
        public async Task<IActionResult> Edit(string? Id)
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
        public async Task<IActionResult> Edit(string Id, ApplicationUserViewModel applicationUsermodel)
        {
            if (Id != applicationUsermodel.Id)
            {
                return NotFound();
            }

            var userOriginal = await _unitOfWork.UserRepository.FirstOrDefaultAsync(Id);
            var userInDatabase = userOriginal;

            if (ModelState.IsValid)
            {
                try
                {
                    if ((!userOriginal.Name.Equals(applicationUsermodel.Name)
                            || (!userOriginal.Email.Equals(applicationUsermodel.Email))))
                    {
                        TempData["ChangedParticipant"] = $"The {userInDatabase.Name} has been changed!";
                    }

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
                return RedirectToAction("Index", "Teachers");
            }
            return View(applicationUsermodel);
        }



        private async Task<bool> ApplicationUserExists(string Id)
        {
            return await _unitOfWork.UserRepository.AnyAsync(Id);
        }



        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _unitOfWork.UserRepository.FindAsync(id);

            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }



        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userWillBeDeleted = await _unitOfWork.UserRepository.FirstOrDefaultAsync(id);   
            var documentsForUserWillBeDeleted = _unitOfWork.DocumentRepository.GetAllAsync().Result.Where(u => u.ApplicationUserId == id);

            foreach (var item in documentsForUserWillBeDeleted)
            {
                item.ApplicationUserId = null;
            }

            TempData["DeleteParticipant"] = $"The {userWillBeDeleted.Name} has been deleted!";
            _unitOfWork.UserRepository.Remove(userWillBeDeleted);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index", "Teachers");
        }
          


        public async Task<List<DaySchemaViewModel>> WeekSchema()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId;

            var activities = await _unitOfWork.ModuleRepository.GetSortedListOfWeeklyActivitiesAsync((int)courseId);

            List<DaySchemaViewModel> weekSchema = await CreateWeeklySchema(UserLoggedIn, activities);

            return weekSchema;
        }



        private async Task<List<DaySchemaViewModel>> CreateWeeklySchema(ApplicationUser UserLoggedIn, IEnumerable<Activity> activities)
        {
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

                TestActivityToSchema(date, daySchema);//Enbart för att testa HTML koden

                foreach (var activity in activities)
                {
                    if (activity.StartDate <= date && activity.EndDate >= date)
                    {
                        var activitySchema = new ActivitySchemaViewModel()
                        {
                            Name = activity.Name,
                            ActivityTypeName = activity.ActivityType.TypeName,
                            DeadLine = activity.Deadline == date
                        };

                        if (activitySchema.DeadLine)
                        {
                            activitySchema.Submitted = await _unitOfWork.DocumentRepository.HasUserSubmittedAssignment(UserLoggedIn, activity);
                        }

                        daySchema.ActivitySchemas.Add(activitySchema);
                    }
                }
                weekSchema.Add(daySchema);
            }

            return weekSchema;
        }



        private static void TestActivityToSchema(DateTime date, DaySchemaViewModel daySchema)
        {
            //Test för Dealine hantering
            if (date.DayOfWeek.ToString() == "Friday")
            {
                var activitySchema = new ActivitySchemaViewModel()
                {
                    Name = "TeST",
                    ActivityTypeName = "Lecture",
                    DeadLine = true,
                    Submitted = true
                };

                daySchema.ActivitySchemas.Add(activitySchema);
            }
        }
    }
}