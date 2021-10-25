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
                Name = UserClicked.Name,
                Email = UserClicked.Email,
                CourseId = UserClicked.CourseId
            };

            return View(model);
        }


        
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( string Id, ApplicationUser applicationUser)
        {
            if (Id != applicationUser.Id)
            {
                return NotFound();
            }

            var userInDatabase = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
        
            if (!Equals(userInDatabase, applicationUser))
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        //DbUpdateConcurrencyException: Database operation expected to affect 1 row(s) but actually affected 0 row(s).
                        //Data may have been modified or deleted since entities were loaded. 
                        //See http://go.microsoft.com/fwlink/?LinkId=527962 for information on understanding and handling optimistic concurrency exceptions.

                        //db.Update(applicationUser);            
                        //await db.SaveChangesAsync();                  

                        db.Users.Update(applicationUser);
                        await db.SaveChangesAsync();
                        //db.Entry(applicationUser).State = EntityState.Modified;

                        // _unitOfWork.UserRepository.Update(applicationUser);
                        //await _unitOfWork.CompleteAsync();
                        //TempData["ChangedParticipant"] = "The participant is changed!";
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
                } 
            }
            return RedirectToAction("Index", "Teachers");

        }


        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, ApplicationUser applicationUser)
        {
            //från databasen
            var appUserFromDatabase = await _unitOfWork.UserRepository.FirstOrDefaultAsync(Id);
              

            var appUsermodel = new ApplicationUser
            {
                Name = applicationUser.Name,
                Email = applicationUser.Email      
            };

            if (!Equals(appUserFromDatabase, appUsermodel))
            {

                if (Id != appUsermodel.Id)
                {
                    return NotFound();
                }
            

                if (ModelState.IsValid)
                {
                    try
                    {
                        _unitOfWork.UserRepository.Update(appUsermodel);
                        await db.SaveChangesAsync();
                        TempData["ChangedVehicle"] = "The vehicle is changed!";
                        return RedirectToAction("Details", new { id = appUsermodel.Id });
                    }
                    catch (DbUpdateConcurrencyException)
                    {                       
                            return NotFound();                     
                        
                    }
                }
            }
            return RedirectToAction("Details", new { id = appUsermodel.Id });
        }
        */




        private async Task<bool> ApplicationUserExists(string Id)
        {
            return await _unitOfWork.UserRepository.AnyAsync(Id);
        }


        /*
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(string? id)
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _unitOfWork.UserRepository.GetWithStringIdIncludesAsyncTest(id, query => query.Include(d => d.Documents),
                query => query.Include(c => c.Courses).ThenInclude(m => m.Documents),
                query => query.Include(m => m.Modules).ThenInclude(m => m.Documents),
                query => query.Include(a => a.Modules).ThenInclude(a => a.Activities).ThenInclude(d => d.Documents)).Where(u=>u.Id.Equals(id));
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
        */
    }
}