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

        public  IActionResult ModuleStartTimeCheck(List<DateTime> ModuleStartDate, List<int?> CourseId)
        {
            for (int i = 0; i < ModuleStartDate.Count; i++)
            {
               var courseStartDate = _unitOfWork.CourseRepository.GetAsync((int)CourseId[i]).Result;

                if (ModuleStartDate[i] < courseStartDate.StartDate)
                {
                    return Json("Start date cannot be earlier then the start date of the course!");
                }
                else if (ModuleStartDate[i] > courseStartDate.EndDate)
                {
                    return Json("Start date cannot be later then the end date of the course!");
                }
            }
            return Json(true);
        }

        public IActionResult ModuleEndTimeCheck(List<DateTime> ModuleEndDate, List<int?> CourseId)
        {
            for (int i = 0; i < ModuleEndDate.Count; i++)
            {
                var courseStartDate = _unitOfWork.CourseRepository.GetAsync((int)CourseId[i]).Result;

                if (ModuleEndDate[i] < courseStartDate.StartDate)
                {
                    return Json("End date cannot be earlier then the start date of the course!");
                }
                else if (ModuleEndDate[i] > courseStartDate.EndDate)
                {
                    return Json("End date cannot be later then the end date of the course!");
                }
            }
            return Json(true);
        }
    }
}
