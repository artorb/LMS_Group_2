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
        private readonly IUnitOfWork _unitOfWork;

        public TeachersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult CreateCourse()
        {
            return View();
        }


        public async Task<IActionResult> Index()
        {
            /*
                ViewData["ActivityTypeId"] = new SelectList(_unitOfWork.ActivityTypes, "Id", "Id");
                ViewData["ModuleId"] = new SelectList(_unitOfWork.Modules, "Id", "Id");
             * Test with ViewData["CourseId"] instead of StudentAndTeacherViewModel
             */
            var courses = await _unitOfWork.CourseRepository.GetAllWithIncludesAsync(m => m.Modules, m => m.Users);

            var viewModels = courses.Select(course => new TeacherLoginViewModel()
                {
                    Course = course,
                    ActiveModuleName = course.Modules.ElementAt(0).Name, //Fix to use StartDate and EndDate, and add null-handler
                    NextModuleName = course.Modules.ElementAt(1).Name, //Fix to use StartDate and EndDate, and add null-handler
                    NumberOfParticipants = course.Users.Count
                })
                .ToList();

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
                    Course = course,
                    ActiveModuleName = course.Modules.ElementAt(0).Name,
                    NextModuleName = course.Modules.ElementAt(1).Name,
                    NumberOfParticipants = course.Users.Count
                };
                viewModels.Add(viewModel);
            }

            return View(nameof(Index), viewModels);
        }


        public async Task<IActionResult> IndexCourseForTeacher(int id)
        {
            ViewData["CourseId"] = id;
            // var model = new StudentsAndTeachersIndexViewModel()
            // {
            //     idFromCourse = id
            // };
            // return View(model);
            return View();
        }
    }
}