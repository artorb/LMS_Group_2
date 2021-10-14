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
        private readonly LmsDbContext _context;
        private readonly IUnitOfWork _unitOfWork;


        public StudentsController(LmsDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {

            return View();

        }

        public IActionResult CourseDetail()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //if (userId == null)
            //{
            //    return NotFound();
            //}          
            var UserLoggedIn = _context.Users.FirstOrDefault(u => u.Id == userId);//Uses _context, need to change
            var courseId = UserLoggedIn.CourseId;//Can throw error if you are already logged in when the application starts
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


        public IActionResult ModuleList()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = _context.Users.FirstOrDefault(u => u.Id == userId);//Uses _context, need to change  

            var courseId = UserLoggedIn.CourseId;//Can throw error if you are already logged in when the application starts   

            var course = _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, m => m.Modules).Result;
            var modulesToCourse = course.Modules;

            return PartialView("GetModuleListPartial", modulesToCourse);
        }

       
        // kolla generic repo för att undvika fler anrop - inte nödvändigt
        public IActionResult ModuleDetail(int Id)
        {
            var module = _unitOfWork.ModuleRepository.GetWithIncludesAsync((int)Id, d => d.Documents, a => a.Activities).Result;
            if (module == null) return NotFound();

            if (module.Activities != null)
            {
                foreach (var item in module.Activities)
                {
                    item.ActivityType = _unitOfWork.ActivityTypeRepository.GetAsync(item.ActivityTypeId).Result;
                }
            }

            var model = new StudentModuleViewModel()
            {
                ModuleName = module.Name,
                ModuleDescription = module.Description,
                ModuleStartDate = module.StartDate,
                ModuleEndDate = module.EndDate,
                Documents = module.Documents,
                Activities = module.Activities    
            };

            return PartialView("GetModuleDetailsPartial", model);
        }
    }
}

