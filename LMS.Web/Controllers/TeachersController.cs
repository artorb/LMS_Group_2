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

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(TeacherCreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activity = new Activity
                {
                    Name = model.ActivityName,
                    Description = model.ActivityDescription,
                    StartDate = model.ActivityStartDate,
                    EndDate = model.ActivityEndDate,
                    Deadline = model.ActivityDeadline,
                    ActivityType = new ActivityType
                    {
                        TypeName = "Laboratory"
                    }
                };
                
                var module = new Module
                {
                    Name = model.ModuleName,
                    Description = model.ModuleDescription,
                    StartDate = model.ModuleStartDate,
                    EndDate = model.ModuleEndDate,
                    Activities = new List<Activity> { activity }
                    // Activities = new List<Activity>()
                };

                var course = new Course
                {
                    Name = model.CourseName,
                    Description = model.CourseDescription,
                    StartDate = model.CourseStartDate,
                    EndDate = model.CourseEndDate,
                    Modules = new List<Module> { module }
                };

                // activity.ActivityType = new ActivityType
                // {
                //     TypeName = "Laboratory"
                // };
                //
                // activity.Module = module;
                // module.Course = course;

                // _unitOfWork.ModuleRepository.Add(module);
                // _unitOfWork.ActivityRepository.Add(activity);
                _unitOfWork.CourseRepository.Add(course);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index()
        {
            /*
                ViewData["ActivityTypeId"] = new SelectList(_unitOfWork.ActivityTypes, "Id", "Id");
                ViewData["ModuleId"] = new SelectList(_unitOfWork.Modules, "Id", "Id");
             * Test with ViewData["CourseId"] instead of StudentAndTeacherViewModel
             */
            var courses = await _unitOfWork.CourseRepository.GetAllWithIncludesAsync(m => m.Modules, m => m.Users);

            var teacherTable = courses.Select(course => new TeacherTableViewModel
                {
                    Course = course,
                    ActiveModuleName =
                        course.Modules.ElementAt(0).Name, //Fix to use StartDate and EndDate, and add null-handler
                    NextModuleName =
                        course.Modules.ElementAt(1).Name, //Fix to use StartDate and EndDate, and add null-handler
                    NumberOfParticipants = course.Users.Count
                })
                .ToList();

            var indexVM = new TeacherIndexViewModel
            {
                CourseToCreate = new TeacherCreateCourseViewModel(),
                TeacherTables = teacherTable
            };

            // return View(viewModels);
            return View(indexVM);
        }
        // public async Task<IActionResult> Search(string courseName)
        // {
        //     var foundCourses = await _unitOfWork.CourseRepository.FindByNameAsync(courseName);
        //
        //     var viewModels = new List<TeacherTableViewModel>();
        //
        //     foreach (var course in foundCourses)
        //     {
        //         var viewModel = new TeacherTableViewModel()
        //         {
        //             Course = course,
        //             ActiveModuleName = course.Modules.ElementAt(0).Name,
        //             NextModuleName = course.Modules.ElementAt(1).Name,
        //             NumberOfParticipants = course.Users.Count
        //         };
        //         viewModels.Add(viewModel);
        //     }
        //
        //     return View(nameof(Index), viewModels);
        // }


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

        public PartialViewResult GetSomeData()
        {
            ViewBag.Message = "Example Data from Server"; //Using ViewBag Just for example, use ViewModel Instead
            return PartialView("~/Views/Teachers/CreateCourse.cshtml");
        }
    }
}