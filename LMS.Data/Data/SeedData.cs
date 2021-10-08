using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(LmsDbContext context)
        {
            if (await context.Courses.AnyAsync()) return;

            await context.Courses.AddRangeAsync(GetCourses());
            await context.SaveChangesAsync();

            await context.Modules.AddRangeAsync(GetModules());
            await context.SaveChangesAsync();
        }

        private static List<Course> GetCourses()
        {
            return new List<Course>
            {
                new Course { Name = "C# Fundamentals", StartDate = new DateTime(2021, 06, 14), EndDate=new DateTime(2021, 06, 14).AddMonths(6)},
                new Course { Name = "Frontend", StartDate = new DateTime(2021, 07, 15), EndDate= new DateTime(2021, 07, 15).AddMonths(6) },
                new Course { Name = "MVC", StartDate = new DateTime(2021, 07, 15), EndDate= new DateTime(2021, 07, 15).AddMonths(6) },
                new Course { Name = "Database", StartDate = new DateTime(2021, 08, 30), EndDate= new DateTime(2021, 08, 30).AddMonths(6) },
                new Course { Name = "API", StartDate = new DateTime(2021, 09, 08), EndDate=new DateTime(2021, 09, 08).AddMonths(6) },
                new Course { Name = "Backend", StartDate = new DateTime(2021, 09, 20), EndDate=new DateTime(2021, 09, 20).AddMonths(6) },
            };
        }

        private static List<Module> GetModules()
        {
            return new List<Module>
            {
                new Module { CourseId = 1, Name = "C# Intro", StartDate = new DateTime(2021, 06, 14) },
                new Module { CourseId = 1, Name = "OOP", StartDate = new DateTime(2021, 06, 22) },
                new Module { CourseId = 1, Name = "Generics", StartDate = new DateTime(2021, 06, 29) },
                new Module { CourseId = 1, Name = "Delegates", StartDate = new DateTime(2021, 07, 02) },
                new Module { CourseId = 2, Name = "HTML", StartDate = new DateTime(2021, 07, 15) },
                new Module { CourseId = 2, Name = "CSS", StartDate = new DateTime(2021, 07, 16) },
                new Module { CourseId = 2, Name = "Bootstrap", StartDate = new DateTime(2021, 08, 03) },
                new Module { CourseId = 2, Name = "JavaScript", StartDate = new DateTime(2021, 08, 06) },
                new Module { CourseId = 3, Name = "MVC Intro", StartDate = new DateTime(2021, 08, 12) },
                new Module { CourseId = 3, Name = "PartialView/ViewModel", StartDate = new DateTime(2021, 08, 19) },
                new Module { CourseId = 3, Name = "Git", StartDate = new DateTime(2021, 08, 20) },
                new Module { CourseId = 4, Name = "SQL Bolt", StartDate = new DateTime(2021, 08, 30) },
                new Module { CourseId = 4, Name = "EntityFramework", StartDate = new DateTime(2021, 08, 31) },
                new Module { CourseId = 5, Name = "API Intro", StartDate = new DateTime(2021, 09, 08) },
                new Module { CourseId = 5, Name = "RESTful APIs", StartDate = new DateTime(2021, 09, 10) },
            };
        }


    }
}
