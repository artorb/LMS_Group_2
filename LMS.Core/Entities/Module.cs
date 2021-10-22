using Lms.Core.Validations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Entities
{
    public class Module : BaseEntity
    {
        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Please enter start date!")]
        [Display(Name = "Start date")]   
        [ModuleStartTimeCheck]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        [ModuleEndTimeCheck]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}