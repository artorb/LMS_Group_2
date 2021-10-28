using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Web.Service;
using Newtonsoft.Json;

namespace Lms.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LmsDbContext _context;


        public ModulesController(IUnitOfWork unitOfWork, IActivityService activityService, LmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _activityService = activityService;
            _context = context;
        }



        public async Task<IActionResult> ModuleDetail(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var module = await _unitOfWork.ModuleRepository
                .GetWithIncludesIdAsync((int)Id, d => d.Documents, a => a.Activities);

            if (module == null) return NotFound();

            if (module.Activities != null)
            {
                foreach (var item in module.Activities)
                {
                    item.ActivityType = _unitOfWork.ActivityTypeRepository.GetAsync(item.ActivityTypeId).Result;
                }
            }

            ViewData["ModuleId"] = module.Id;
            if (User.IsInRole("Student"))
            {
                var model = new StudentModuleViewModel()
                {
                    Id = module.Id,
                    ModuleName = module.Name,
                    ModuleDescription = module.Description,
                    ModuleStartDate = module.StartDate,
                    ModuleEndDate = module.EndDate,
                    Documents = module.Documents,
                    Activities = module.Activities,
                    Status = _activityService.GetStatusForStudentModule(module, userId).Result
                };
                return PartialView("~/Views/Students/GetModuleDetailsPartial.cshtml", model);
            }
            else
            {
                var model = new StudentModuleViewModel()
                {
                    Id = module.Id,
                    ModuleName = module.Name,
                    ModuleDescription = module.Description,
                    ModuleStartDate = module.StartDate,
                    ModuleEndDate = module.EndDate,
                    Documents = module.Documents,
                    Activities = module.Activities,
                    Status = "Uploaded"
                };
                return PartialView("~/Views/Students/GetModuleDetailsPartial.cshtml", model);
            }
        }



        public async Task<IActionResult> ModuleList(int? idFromCourse)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId;

            var course = (idFromCourse == null)
                ?
                //Student
                _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, m => m.Modules).Result
                :
                //Teacher
                _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)idFromCourse, m => m.Modules).Result;


            var modulesToCourse = course.Modules.OrderBy(c=>c.StartDate);

            ViewData["ModuleCourseId"] = course.Id;

            return PartialView("~/Views/Students/GetModuleListPartial.cshtml", modulesToCourse);
        }



        // GET: Modules
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.ModuleRepository.GetAllAsync());
        }



        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _unitOfWork.ModuleRepository
                .FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
            //return PartialView(module);
        }


        public IActionResult Create(int id)
        {
            ViewData["ModuleCourseId"] = id;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuleListViewModel viewModel, int id)
        {
            var course = _unitOfWork.CourseRepository.GetAsync(id).Result;
            var returnUrl = Url.Content("/Teachers/IndexCourseForTeacher/" + id);
            var res = new List<Module>();
            foreach (var item in viewModel.Modules)
            {
                if (item.ModuleStartDate < course.StartDate || item.ModuleEndDate > course.EndDate)
                {
                    if (item.ModuleStartDate < course.StartDate)
                    {
                        TempData["ModuleStartDateCreationError"] = $"The module was not created. The Start Date you used was before the start of the course start date! Try again!";
                    }
                    if (item.ModuleEndDate > course.EndDate)
                    {
                        TempData["ModuleEndDateCreationError"] = "The module was not created. The End Date you used was later then the course end date! Try again!";
                    }
                    return RedirectToAction("Create", id);
                }

                var temp = new Module
                {
                    Name = item.ModuleName,
                    Description = item.ModuleDescription,
                    StartDate = item.ModuleStartDate,
                    EndDate = item.ModuleEndDate,
                    CourseId = id,
                    Course = item.Course
                };

                res.Add(temp);
                _unitOfWork.ModuleRepository.Add(temp);
                await _unitOfWork.CompleteAsync();
                var idForActivity = temp.Id;

                //return RedirectToAction("Create", "Activities", new { @id = idForActivity });
            }
            return LocalRedirect(returnUrl);
            //return RedirectToAction("Index", "Teachers");
            //return View();
        }


        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _unitOfWork.ModuleRepository.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }



        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (id != module.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ModuleRepository.Update(module);
                    await _unitOfWork.CompleteAsync();
                    TempData["ChangedModule"] = $"The {module.Name} has been changed!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ModuleExists(module.Id))
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
            return View(module);
        }



        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _unitOfWork.ModuleRepository.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }



        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var module = await _unitOfWork.ModuleRepository.GetWithIncludesAsyncTest(id, query => query.Include(d => d.Documents),
              query => query.Include(a => a.Activities).ThenInclude(d => d.Documents));

            TempData["DeleteModule"] = $"The {module.Name} has been deleted!";
            _unitOfWork.ModuleRepository.Remove(module);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index", "Teachers");
        }


        private async Task<bool> ModuleExists(int id)
        {
            return await _unitOfWork.ModuleRepository.AnyAsync(id);
        }
    }
}