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
            var allCourses =
                await _unitOfWork.CourseRepository.GetAllWithIncludesAsync(course => course.Modules);

            var activityTest = _unitOfWork.ActivityRepository.GetAllWithIncludesAsync(x => x.ActivityType).Result;

            var moduleTest = _unitOfWork.ModuleRepository.GetAllWithIncludesAsync(
                x => x.Activities, x => x.Course).Result;

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
                CourseName = course.Name,
                CourseDescription = course.Description,
                CourseStartDate = course.StartDate,
                CourseEndDate = course.EndDate,
                Documents = course.Documents
            };
            return PartialView("~/Views/Students/_CourseDetailsPartial.cshtml", model);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CourseRepository.Add(course);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(course);
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

                return RedirectToAction(nameof(Index));
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

        private async Task<bool> CourseExists(int id)
        {
            return await _unitOfWork.CourseRepository.AnyAsync(id);
        }
    }
}