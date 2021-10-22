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
using Lms.Data.Data;
using Lms.Data.Repositories;
using Lms.Core.Repositories;
using Lms.Web.Service;
using Microsoft.AspNetCore.Authorization;

namespace Lms.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityService _activityService;

        public ActivitiesController(IUnitOfWork unitOfWork, IActivityService activityService)
        {
            _unitOfWork = unitOfWork;
            _activityService = activityService;
        }

        public async Task<IActionResult> ActivityDetail(int Id)
        {
            var activity = await _unitOfWork.ActivityRepository
                .GetWithIncludesIdAsync((int)Id, d => d.Documents, a => a.ActivityType);
            if (activity == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var model = new StudentActivityViewModel()
            {
                Id = activity.Id,
                ActivityName = activity.Name,
                ActivityTypes = activity.ActivityType,
                ActivityDescription = activity.Description,
                ActivityStartDate = activity.StartDate,
                ActivityEndDate = activity.EndDate,
                Documents = activity.Documents,
                Status = _activityService.GetStatusForStudentActivity(activity, userId).Result
            };

            return PartialView("~/Views/Students/GetActivityDetailsPartial.cshtml", model);
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            //var activities = await _unitOfWork.ActivityRepository.GetAllAsync();

            var activity = await _unitOfWork.ActivityRepository.GetWithIncludesAsync
                (a => a.Include(act => act.ActivityType).Include(activity => activity.Module));
            return View(activity.ToList());
        }
        //public async Task<IEnumerable<Activity>> ActivitesWithTypeAndModule()
        //{
        //    return await _context.Activities.Include(a => a.ActivityType).Include(a => a.Module).ToListAsync();
        //}


        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _unitOfWork.ActivityRepository.GetWithIncludesAsync
                (a => a.Include(act => act.ActivityType).Include(activity => activity.Module));

            var found = activity.FirstOrDefault(c => c.Id == id);
            if (found == null)
            {
                return NotFound();
            }

            return View(found);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Description,StartDate,EndDate,Deadline,ModuleId,ActivityTypeId")]
            Activity activity)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ActivityRepository.Add(activity);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _unitOfWork.ActivityRepository.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,Description,StartDate,EndDate,Deadline,ModuleId,ActivityTypeId")]
            Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.ActivityRepository.Update(activity);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id).Result)
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

            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _unitOfWork.ActivityRepository.GetWithIncludesAsync
            (a => a.Include(act => act.ActivityType).Include(activity => activity.Module)
                .Include(ac => ac.Documents));

            var found = activity.FirstOrDefault(c => c.Id == id);

            if (found == null)
            {
                return NotFound();
            }

            return View(found);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity =
                await _unitOfWork.ActivityRepository.GetWithIncludesIdAsync(id, a => a.Documents);
            var documents = activity.Documents;

            // TODO FIXME
            foreach (var doc in documents)
            {
                _unitOfWork.DocumentRepository.Remove(doc);
            }
            
            _unitOfWork.ActivityRepository.Remove(activity);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ActivityExists(int id)
        {
            return await _unitOfWork.ActivityRepository.AnyAsync(id);
        }
    }
}