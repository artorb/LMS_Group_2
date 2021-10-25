using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                select new StudentCommonCourseViewModel { 
                    Id = user.Id,
                    StudentName = user.Name, 
                    Email = user.Email }).ToList();
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
        public async Task<IActionResult> Edit( string Id, ApplicationUserViewModel applicationUsermodel)
        {
            if (Id != applicationUsermodel.Id)
            {
                return NotFound();
            }
            var userInDatabase = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);          
         
            if (ModelState.IsValid)
                {
                    try
                    {

                    userInDatabase.Email = applicationUsermodel.Email;
                    userInDatabase.Name = applicationUsermodel.Name;
                    userInDatabase.UserName = applicationUsermodel.Name;
                    userInDatabase.NormalizedUserName = applicationUsermodel.Name;
  

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

            //if student uploaded something : problem with documents. ApplicationUserId should be NULL
            var documentsForUserWillBeDeleted = _unitOfWork.DocumentRepository.GetAllAsync().Result.Where(u=>u.ApplicationUserId == id);


            foreach (var item in documentsForUserWillBeDeleted)
            {
                item.ApplicationUserId = null;
            }

            TempData["DeleteParticipant"] = $"The {userWillBeDeleted.Name} has been deleted!";
            _unitOfWork.UserRepository.Remove(userWillBeDeleted);
            await _unitOfWork.CompleteAsync();  
            return RedirectToAction("Index", "Teachers");
        }
        
    }
}