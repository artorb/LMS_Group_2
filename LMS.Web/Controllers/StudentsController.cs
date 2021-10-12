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
            var UserLoggedIn = _context.Users.FirstOrDefault(u=>u.Id==userId);
            var courseId = UserLoggedIn.CourseId;
            var course = _unitOfWork.CourseRepository.GetWithIncludesAsync((int)courseId, d=>d.Documents).Result;

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




    }
}
