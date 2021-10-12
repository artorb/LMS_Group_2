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
    public class TeacherController : Controller
    {
        private readonly LmsDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public TeacherController(LmsDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var courses = _unitOfWork.CourseRepository.GetAllWithIncludesAsync(m => m.Modules, m=>m.Users).Result;

            var viewModels = new List<TeacherLoginViewModel>();

            foreach (var course in courses)
            {
                var viewModel = new TeacherLoginViewModel()
                {
                    CourseName = course.Name,
                    ActiveModuleName = course.Modules.ElementAt(0).Name,
                    NextModuleName = course.Modules.ElementAt(1).Name,
                    NumberOfParticipants = course.Users.Count
                    //NumberOfParticipants = 1
                };
                viewModels.Add(viewModel);
            }
            return View(viewModels);
        }
    }
}
