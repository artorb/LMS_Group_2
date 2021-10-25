using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
    public class StudentCommonCourseViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Name")] 
        public string StudentName { get; set; }


        [Display(Name = "Email")] 
        public string Email { get; set; }
    }
}