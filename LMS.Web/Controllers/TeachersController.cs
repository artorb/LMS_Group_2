using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly LmsDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public TeachersController(LmsDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }



        public async Task<IActionResult> Index()
        {
            var courses = await _unitOfWork.CourseRepository.GetAllWithIncludesAsync(m => m.Modules, m => m.Users);

            var viewModels = new List<TeacherLoginViewModel>();

            foreach (var course in courses)
            {
                var viewModel = new TeacherLoginViewModel()
                {
                    CourseName = course.Name,
                    ActiveModuleName = course.Modules.ElementAt(0).Name,//Fix to use StartDate and EndDate, and add null-handler
                    NextModuleName = course.Modules.ElementAt(1).Name,//Fix to use StartDate and EndDate, and add null-handler
                    NumberOfParticipants = course.Users.Count
                };
                viewModels.Add(viewModel);
            }
            return View(viewModels);
        }

        public async Task<IActionResult> Search(string courseName)
        {

            var foundCourses = await _unitOfWork.CourseRepository.FindByNameAsync(courseName);

            var viewModels = new List<TeacherLoginViewModel>();

            foreach (var course in foundCourses)
            {
                var viewModel = new TeacherLoginViewModel()
                {
                    CourseName = course.Name,
                    ActiveModuleName = course.Modules.ElementAt(0).Name,
                    NextModuleName = course.Modules.ElementAt(1).Name,
                    NumberOfParticipants = course.Users.Count
                };
                viewModels.Add(viewModel);
            }

            return View(nameof(Index), viewModels);
        }
    }
}
