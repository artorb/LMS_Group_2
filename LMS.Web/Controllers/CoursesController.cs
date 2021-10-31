using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Data.Repositories;
using Lms.Core.Repositories;
using Lms.Web.Extensions;
using System.Security.Claims;
using Lms.Core.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Lms.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
  

    public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;         
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            /*GetAll with params example */
            var allCourses = await _unitOfWork.CourseRepository.GetAllWithIncludesAsync(course => course.Modules);

            var activityTest = _unitOfWork.ActivityRepository.GetAllWithIncludesAsync(x => x.ActivityType).Result;

            var moduleTest = _unitOfWork.ModuleRepository.GetAllWithIncludesAsync(x => x.Activities, x => x.Course).Result;

            return View(allCourses);
        }



        public async Task<IActionResult> CourseDetails(int? idFromCourse)
        {            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);        

            var courseId = UserLoggedIn.CourseId;

            var course = (idFromCourse == null)
                ? await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId,
                    d => d.Documents.Where(m => m.ApplicationUser == null))
                : await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)idFromCourse,
                    d => d.Documents.Where(m => m.ApplicationUser == null));

            ViewData["CourseId"] = course.Id;
            var model = new StudentCourseViewModel()
            {
                Id = course.Id,
                CourseName = course.Name,
                CourseDescription = course.Description,
                CourseStartDate = course.StartDate,
                CourseEndDate = course.EndDate,
                Documents = course.Documents
            };        
            return View("~/Views/Students/Index.cshtml", model);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {   
                var temp = new Course
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate
                };

                _unitOfWork.CourseRepository.Add(temp);        
                await _unitOfWork.CompleteAsync();
                var id = temp.Id;           
     
                return RedirectToAction("Create", "Modules", new { @id = id });
            }
            return View();
        }



        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CourseRepository.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }



        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CourseRepository.Update(course);
                    await _unitOfWork.CompleteAsync();
           
                    TempData["ChangedCourse"] = $"The {course.Name} has been changed!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id).Result)
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
            return View(course);
        }




        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CourseRepository.FindAsync(id);
           
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

            var courseWillBeDeleted = await _unitOfWork.CourseRepository.GetAsync(id);

            var moduleToCourse = _unitOfWork.ModuleRepository.GetAllAsync().Result.Where(u => u.CourseId==id);
            foreach(var item in moduleToCourse)
            {
                _unitOfWork.ModuleRepository.Remove(item);
            }

            TempData["DeleteCourse"] = $"The {course.Name} has been deleted!";
            _unitOfWork.CourseRepository.Remove(course);
            await _unitOfWork.CompleteAsync();
          
            return RedirectToAction("Index", "Teachers");
        }



        private async Task<bool> CourseExists(int id)
        {
            return await _unitOfWork.CourseRepository.AnyAsync(id);
        }
    }
}