using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Lms.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Lms.Core.Repositories;

namespace Lms.Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = Core.Entities.UserRoles.Teacher)]
    public class RegisterTeacherModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterTeacherModel(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public static IEnumerable<Course> Courses { get; private set; }
        public IEnumerable<string> UserValues { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }


            [Display(Name = "Full Name")]
            public string Name
            {
                get
                {
                    return string.Concat(FirstName[0].ToString().ToUpper(), FirstName.AsSpan(1)) + " "
                        + string.Concat(LastName[0].ToString().ToUpper(), LastName.AsSpan(1));
                }
            }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            public string Password { get { return "12"; } }

            //Used to assign the role upon creation of the user
            [Required]
            public string Role { get { return UserRoles.Teacher; } }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Courses = await _unitOfWork.CourseRepository.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Identity/Account/Register" + UserRoles.Teacher);
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Name = Input.Name };
                var result1 = await _userManager.CreateAsync(user, Input.Password);
                var result2 = await _userManager.AddToRoleAsync(user, Input.Role);//Tries to assign the role in "Input.Role" to "user"

                if (result1.Succeeded && result2.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    TempData["UserName"] = Input.Name;
                    return LocalRedirect(returnUrl);

                }
                foreach (var error in result1.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in result1.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


    }
}
