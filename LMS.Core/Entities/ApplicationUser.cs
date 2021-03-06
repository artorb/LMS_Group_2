using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lms.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {

        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        [RegularExpression("[-a-zA-Z]+", ErrorMessage = "Invalid name!")]
        public string Name { get; set; }


        //Navigations:
        public int? CourseId { get; set; }
        public Course Course { get; set; }       
        
        public ICollection<Document> Documents { get; set; }
    }



    public static class UserRoles
    {
        //The diffferent roles as string constants
        public const string Student = "Student";
        public const string Teacher = "Teacher";

        //The list of all the role constants
        public static List<string> RolesList
        {
            get
            {
                return typeof(UserRoles).GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Where(x => x.IsLiteral && !x.IsInitOnly)
                    .Select(x => x.GetValue(null)).Cast<string>().ToList();
            }
        }
    }
}