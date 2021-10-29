using Lms.Core.Entities;
using Lms.Core.Models;
using Lms.Core.Models.ViewModels;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;



namespace Lms.Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public static bool UserLoggedIn { get; private set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId) != null;

            if (UserLoggedIn == false)//Checks that the user is logged in as someone from the database.
            {
                return Redirect("~/Identity/Account/Login");
            }

            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Students");
                //var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
                //var courseId = UserLoggedIn.CourseId;
                //return RedirectToAction("CourseDetails", "Courses", new { idFromCourse = courseId });
            }
            return Error();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> PersonnalList()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserLoggedIn = await _unitOfWork.UserRepository.FirstOrDefaultAsync(userId);
            if (UserLoggedIn == null)//Checks that the user is logged in as someone from the database.
            {
                return Redirect("~/Identity/Account/Login");
            }

            List<ApplicationUser> personnalList = new();
            foreach (var role in UserRoles.RolesList)
            {
                if (role != UserRoles.Student)
                {
                    personnalList.AddRange((List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(role));
                }
            }
            List<PersonnalListViewModel> personnalView = new();
            foreach (var personnal in personnalList)
            {
                personnalView.Add(new PersonnalListViewModel()
                {
                    Id = personnal.Id,
                    Name = personnal.Name,
                    Email = personnal.Email,
                    RoleName = (List<string>)await _userManager.GetRolesAsync(personnal)
                });
            }
            return View(personnalView);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
