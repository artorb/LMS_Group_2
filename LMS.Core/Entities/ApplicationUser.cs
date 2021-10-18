using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace Lms.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        //Navigations:
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; }
    }

    public static class UserRoles
    {
        public const string Student = "Student";
        public const string Teacher = "Teacher";
        public static List<string> RolesList { get; }

        static UserRoles()
        {
            RolesList = typeof(UserRoles).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => x.GetValue(null)).Cast<string>().ToList();
        }
    }
}