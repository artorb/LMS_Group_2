using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Validations
{  
    public class ModuleStartTimeCheckAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        public ModuleStartTimeCheckAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ModuleStartTimeCheckAttribute()
        {
            ErrorMessage = "Start date cannot be earlier then the start date of the course!";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Module)validationContext.ObjectInstance;
            var courseStartDate = _unitOfWork.CourseRepository.GetAsync(model.CourseId).Result.StartDate;
            if (model.StartDate <= courseStartDate)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-timecheck", ErrorMessage);
        }
    }
}
