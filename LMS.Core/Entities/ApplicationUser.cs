﻿using System;
using System.Collections;
using System.Collections.Generic;
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
        public const string Teacher = "Teacher";
        public const string Student = "Student";
    }

    public enum InputRoles
    {
        Teacher,
        Student
    }
}