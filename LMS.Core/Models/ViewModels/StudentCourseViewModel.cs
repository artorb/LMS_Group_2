using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class StudentCourseViewModel
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime CourseEndDate { get; set; }   

        public ICollection<Document> Documents  { get; set; }

    }
}
