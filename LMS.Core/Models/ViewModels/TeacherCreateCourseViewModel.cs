using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lms.Core.Models.ViewModels
{
    public class TeacherCreateCourseViewModel
    {
        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        public string CourseName { get; set; }


        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        public string CourseDescription { get; set; }


        [Required(ErrorMessage = "Please enter start date!")]
        [Display(Name = "Start date")]
        // [CourseStartTimeCheck]      
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CourseStartDate { get; set; }


        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        // [CourseEndTimeCheck]       
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CourseEndDate { get; set; }
        
        
        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        public string ModuleName { get; set; }
        
        
        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        public string ModuleDescription { get; set; }
        
        
        [Required(ErrorMessage = "Please enter start date!")]
        [Display(Name = "Start date")]
        // [ModuleStartTimeCheck]      
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime ModuleStartDate { get; set; }
        
        
        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        // [ModuleEndTimeCheck]       
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime ModuleEndDate { get; set; }
        
        
        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        public string ActivityName { get; set; }
        
        
        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        public string ActivityDescription { get; set; }



        [Required(ErrorMessage = "Please enter the type!")]
        // FIXME from hardcoding
        public IEnumerable<SelectListItem> ActivityType => new List<SelectListItem>()
        {
            new()
            {
                Text = "Laboratory",
                Value = "1"
            },
            new()
            {
                Text = "Lecture",
                Value = "2"
            },
            new()
            {
                Text = "Assignment",
                Value = "3"
            }
        };
        
        
        [Required(ErrorMessage = "Please enter start date!")]
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        // [AcitivtyStartTimeCheck]
        public DateTime ActivityStartDate { get; set; }
        
        
        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        // [ActivityEndTimeCheck]
        public DateTime ActivityEndDate { get; set; }
        
        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "Activity Deadline")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime ActivityDeadline { get; set; }
        // [ActivityEndTimeCheck]
        // [NotNull]
        // private DateTime? activityDeadline = null;
        // public DateTime ActivityDeadline
        // {
        //     get
        //     {
        //         if (activityDeadline != null) return (DateTime)activityDeadline;
        //         return default;
        //     }
        //     set
        //     {
        //         if (value - ActivityStartDate <= TimeSpan.Zero) return;
        //         if (value < ModuleEndDate)
        //         {
        //             activityDeadline = value;
        //         }
        //     }
        // }
    }
}