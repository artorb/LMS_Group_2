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
    public class CourseEndTimeCheckAttribute : ValidationAttribute, IClientModelValidator
        {

            public CourseEndTimeCheckAttribute()
            {
                ErrorMessage = "End date cannot be before start date!";
            }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var model = (Course)validationContext.ObjectInstance;
                if (model.EndDate < model.StartDate)
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