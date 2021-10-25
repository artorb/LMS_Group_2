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

namespace Lms.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IUnitOfWork _unitOfWork;

        public ModulesController(IUnitOfWork unitOfWork, IActivityService activityService)
        {
            _unitOfWork = unitOfWork;
            _activityService = activityService;
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


            var modulesToCourse = course.Modules;
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

        // GET: Modules/Create
        public IActionResult Create()
        {
            //return View();
            return PartialView();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Module @module)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ModuleRepository.Add(module);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            //return View(module);
            return PartialView(module);
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ModuleExists(module.Id))
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
            //var module = await _unitOfWork.ModuleRepository.GetWithIncludesIdAsync(id, a => a.Documents);
            //var documents = module.Documents;
            var module = await _unitOfWork.ModuleRepository.GetWithIncludesAsyncTest(id, query => query.Include(d => d.Documents),      
              query => query.Include(a => a.Activities).ThenInclude(d => d.Documents));


            // TODO FIXME
            //foreach (var doc in documents)
            //{
            //    _unitOfWork.DocumentRepository.Remove(doc);
            //}




            _unitOfWork.ModuleRepository.Remove(module);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ModuleExists(int id)
        {
            return await _unitOfWork.ModuleRepository.AnyAsync(id);
        }
    }
}