using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Lms.Web.Controllers
{
    public class ValidationsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult ModuleStartTimeCheck(DateTime ModuleStartDate, int? CourseId)
        {
            var courseStartDate = _unitOfWork.CourseRepository.GetAsync((int)CourseId).Result.StartDate;
            if (ModuleStartDate <= courseStartDate)
            {
                return Json($"Start date cannot be earlier then the start date of the course!");
            }
            else
            {
                return Json(true);
            }
        }
    }
}
