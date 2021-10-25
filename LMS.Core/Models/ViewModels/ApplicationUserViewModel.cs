using Lms.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
  public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        [RegularExpression("[-a-zA-Z]+", ErrorMessage = "Invalid name!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }



        //Navigations:
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
