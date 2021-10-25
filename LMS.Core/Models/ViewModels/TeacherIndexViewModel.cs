using System.Collections.Generic;

namespace Lms.Core.Models.ViewModels
{
    public class TeacherIndexViewModel
    {
        public IEnumerable<TeacherTableViewModel> TeacherTables { get; set; }
        public TeacherCreateCourseViewModel CourseToCreate { get; set; }
    }
}