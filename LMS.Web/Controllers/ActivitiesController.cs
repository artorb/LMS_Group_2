﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace Lms.Web.Controllers
{
 
    public class ActivitiesController : Controller
    {
        private readonly LmsDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ActivitiesController(LmsDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            //var activities = await _unitOfWork.ActivityRepository.GetAllAsync();

            var lmsDbContext = _context.Activities.Include(a => a.ActivityType).Include(a => a.Module);
            return View(await lmsDbContext.ToListAsync());
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

            var activity = await _context.Activities
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Id");
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Deadline,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id", activity.ModuleId);
            return View(activity);
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
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Id", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var activity = await _context.Activities.FindAsync(id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
