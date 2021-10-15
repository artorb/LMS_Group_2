using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class StudentCourseViewModel
    {
        [Display(Name = "Course:")]
        public string CourseName { get; set; }

        [Display(Name = "Description:")]
        public string CourseDescription { get; set; }
        
        [Display(Name = "Start date:")]
        public DateTime CourseStartDate { get; set; }

        [Display(Name = "End date:")]
        public DateTime CourseEndDate { get; set; }   

        [Display(Name = "Documents")]
        public ICollection<Document> Documents  { get; set; }

    }
}
