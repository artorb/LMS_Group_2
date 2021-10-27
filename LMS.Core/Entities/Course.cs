using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lms.Core.Validations;

namespace Lms.Core.Entities
{
    public class Course : BaseEntity
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
        //[CourseStartTimeCheck]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        //[CourseEndTimeCheck]
        [DataType(DataType.Date)]       
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        public ICollection<Document> Documents { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        //public ICollection<Module> Modules { get; set; }

        public List<Module> Modules { get; set; }

    }
}