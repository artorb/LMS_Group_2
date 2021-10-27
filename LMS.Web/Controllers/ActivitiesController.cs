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
        private readonly LmsDbContext db;
        public static Task<IEnumerable<SelectListItem>> activityTypes { get; private set; }

        public ActivitiesController(IUnitOfWork unitOfWork, IActivityService activityService, LmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _activityService = activityService;
            db = context;
        }

        public async Task<IActionResult> ActivityDetail(int Id)
        {
            var activity = await _unitOfWork.ActivityRepository
                .GetWithIncludesIdAsync((int)Id, d => d.Documents, a => a.ActivityType);
            if (activity == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ViewData["ActivityId"] = activity.Id;
            ViewData["userId"] = userId;
            var model = new StudentActivityViewModel()
            {
                Id = activity.Id,
                UserId = userId,
                ActivityName = activity.Name,
                ActivityTypes = activity.ActivityType,
                ActivityDescription = activity.Description,
                ActivityStartDate = activity.StartDate,
                ActivityDeadline = activity.Deadline,
                ActivityEndDate = activity.EndDate,
                Documents = activity.Documents,
                Status = _activityService.GetStatusForStudentActivity(activity, userId).Result
            };

            return PartialView("~/Views/Students/GetActivityDetailsPartial.cshtml", model);
        }



        // GET: Activities
        public async Task<IActionResult> Index()       
        {   
            var activity = await _unitOfWork.ActivityRepository.GetWithIncludesAsync
                (a => a.Include(act => act.ActivityType).Include(activity => activity.Module));
            return View(activity.ToList());
        }



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



        public IActionResult Create()
        {
            activityTypes = GetAllActivityTypesAsync();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityListViewModel viewModel, int Id)
        {         

            var res = new List<Activity>();
            foreach (var item in viewModel.Activities)
            {
                var temp = new Activity
                {
                    Name = item.ActivityName,
                    Description = item.ActivityDescription,
                    StartDate = item.ActivityStartDate,
                    EndDate = item.ActivityEndDate,
                    Deadline = item.ActivityDeadline,                   
                    ActivityTypeId = item.ActivityTypeId,
                    ModuleId = Id,
                    Module = item.Module
                };

                res.Add(temp);
                _unitOfWork.ActivityRepository.Add(temp);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index", "Courses");
            }
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
        


        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
         
            var activity = await _unitOfWork.ActivityRepository.FindAsync(id);
            var model = new ChangeActivityViewModel
            {
                Name = activity.Name,
                Description = activity.Description,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                Deadline = activity.Deadline,
                ActivityType = await GetAllActivityTypesAsync(),
                ActivityTypeId = activity.ActivityTypeId,
                ModuleId = activity.ModuleId
            };

            if (activity == null)
            {
                return NotFound();
            }

            // return View(activity);
            return View("Change", model);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Deadline,ModuleId,ActivityTypeId")] Activity activity)
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
                    TempData["ChangedActivity"] = $"The {activity.Name} has been changed!";
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
                return RedirectToAction("Index", "Teachers");
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

            var activity = await _unitOfWork.ActivityRepository.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }
    

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var activity = await _unitOfWork.ActivityRepository.GetWithIncludesAsyncTest(id, query => query.Include(d => d.Documents));


            TempData["DeleteCourse"] = $"The {activity.Name} has been deleted!";
            _unitOfWork.ActivityRepository.Remove(activity);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index", "Teachers");
        }

        private async Task<bool> ActivityExists(int id)
        {
            return await _unitOfWork.ActivityRepository.AnyAsync(id);
        }
    }
}