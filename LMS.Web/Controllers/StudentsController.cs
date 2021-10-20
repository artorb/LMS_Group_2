using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Lms.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;
using Lms.Core.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LmsDbContext _context;

        public StudentsController(LmsDbContext context, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var schema = await VeckoSchema();
            return View(schema);
        }

        public async Task<IActionResult> CourseDetail()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId;
            var course = _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d => d.Documents).Result;

            var model = new StudentCourseViewModel()
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CourseStartDate = course.StartDate,
                CourseEndDate = course.EndDate,
                Documents = course.Documents
            };
            return PartialView("GetCourseDetailsPartial", model);
        }

        public async Task<List<VeckoSchemaViewModel>> VeckoSchema()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId;

            var modules = await _context.Modules
                .Include(m => m.Activities).ThenInclude(a => a.ActivityType)
                .Where(m => m.CourseId == courseId && m.EndDate > DateTime.Today && m.StartDate <= DateTime.Today.AddDays(7))
                .OrderBy(m => m.StartDate).ToListAsync();

            List<Activity> activities = new();
            foreach (var module in modules)
            {
                var act = module.Activities
                    .Where(a => a.EndDate > DateTime.Today && a.StartDate <= DateTime.Today.AddDays(7))
                    .OrderBy(a => a.StartDate).ToList();
                activities.AddRange(act);
            }

            List<VeckoSchemaViewModel> veckoSchema = new();

            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.Today.AddDays(i);

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;

                foreach (var activity in activities)
                {
                    if (activity.StartDate <= date && activity.EndDate >= date)
                    {
                        var dagsSchema = new VeckoSchemaViewModel()
                        {
                            WeekDay = date.DayOfWeek.ToString(),
                            Name = activity.Name,
                            ActivityTypeName = activity.ActivityType.TypeName,
                            DeadLine = activity.Deadline == date ? true : false
                        };
                        veckoSchema.Add(dagsSchema);
                    }
                }
            }

            return veckoSchema;
        }
    }
}
