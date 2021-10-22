using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityService _activityService;

        public StudentsController(IUnitOfWork unitOfWork, IActivityService activityService)
        {
            _unitOfWork = unitOfWork;
            _activityService = activityService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> CourseStudentsDetails(int? idFromCourse)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            var courseId = UserLoggedIn.CourseId;

            var course = (idFromCourse == null) ?
                //Student
            await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)courseId, d => d.Users) :
            //Teacher
            await _unitOfWork.CourseRepository.GetWithIncludesIdAsync((int)idFromCourse, d => d.Users);

            var models = (from user in course.Users            
                select new StudentCommonCourseViewModel { StudentName = user.Name, Email = user.Email }).ToList();
            return PartialView("_CourseStudentsPartial", models);
        }
    }
}