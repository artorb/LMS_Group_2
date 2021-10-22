using Lms.Core.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Validations
{
    public class CourseStartTimeCheckAttribute: ValidationAttribute, IClientModelValidator
    {

        public CourseStartTimeCheckAttribute()
        {
            ErrorMessage = "Start date cannot be earlier than today!";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Course)validationContext.ObjectInstance;
            if (model.StartDate < DateTime.Now)
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
