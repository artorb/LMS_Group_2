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

        public async Task<IActionResult> CourseDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); //Uses _context, need to change
            var courseId =
                UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts
            var course = await _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d => d.Documents);

            var model = new StudentCourseViewModel()
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CourseStartDate = course.StartDate,
                CourseEndDate = course.EndDate,
                Documents = course.Documents
            };
            return PartialView("_CourseDetailsPartial", model);
        }

        public async Task<IActionResult> ModuleDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); //Uses _context, need to change
            var courseId =
                UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts
            var course = await _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d => d.Documents);

            // var model = new StudentModuleViewModel()
            // {
            //     Information = "",
            //     StartDate = null,
            //     EndDate = null,
            //     Activity = null,
            //     Documents = null,
            // };
            // return PartialView("_CourseModulePartial", model);
            return null;
        }


        public async Task<IActionResult> CourseStudentsDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var UserLoggedIn =
                // await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); //Uses _context, need to change
                await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId =
                UserLoggedIn.CourseId; //Can throw error if you are already logged in when the application starts
            var course = await _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d => d.Users);

            var models = (from user in course.Users
                where user.Id != userId
                select new StudentCommonCourseViewModel { StudentName = user.Name, Email = user.Email }).ToList();
            return PartialView("_CourseStudentsPartial", models);
        }
    }
}