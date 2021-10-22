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
        private readonly LmsDbContext db;

        public StudentsController(IUnitOfWork unitOfWork, IActivityService activityService, LmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _activityService = activityService;
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
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
                select new StudentCommonCourseViewModel { StudentName = user.Name, Email = user.Email }).ToList();
            return PartialView("_CourseStudentsPartial", models);
        }



        // GET: Students/Details/5
        public async Task<IActionResult> Details(string? Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);

            //get the participant with courseId
            //var userWithCourse = db.Users.Where(u => u.Id == userId).Include(c => c.Course);

         
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
        public async Task<IActionResult> Edit(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.UserRepository.FindAsync(Id);
            var model = new ApplicationUserViewModel
            {
                Name = user.Name,
                Email = user.Email,               
                CourseId = user.CourseId              
            };

            if (user == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, [Bind("Id,Name, CourseId")] ApplicationUser applicationUser)
        {
            if (Id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(applicationUser);
        }

        private async Task<bool> ApplicationUserExists(string Id)
        {
            return await _unitOfWork.UserRepository.AnyAsync(Id);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CourseRepository.FindAsync(id);
            // var course =
            // //     await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)id, c => c.Modules, c => c.Documents);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _unitOfWork.CourseRepository.GetWithIncludesAsyncTest(id, query => query.Include(d => d.Documents),
                query => query.Include(m => m.Modules).ThenInclude(m => m.Documents),
                query => query.Include(a => a.Modules).ThenInclude(a => a.Activities).ThenInclude(d => d.Documents));
            // var documents = course.;

            // TODO FIXME
            // foreach (var doc in documents)
            // {
            //     _unitOfWork.DocumentRepository.Remove(doc);
            // }
            _unitOfWork.CourseRepository.Remove(course);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}