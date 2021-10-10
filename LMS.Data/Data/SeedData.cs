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
        private static readonly Random _random = new Random(10);

        private const int CourseCapacity = 10;
        private static readonly Dictionary<string ,string> CourseNamePool = new(CourseCapacity) /* Name - Description pair */
        {
            {"Android Course", "Expand your mobile app reach through this Android application development and programming training. Android's open source platform offers compatibility with a wide range of devices, which provide global access to the mobile market."},
            {"Big Data", "The course gives an overview of the Big Data phenomenon, focusing then on extracting value from the Big Data using predictive analytics techniques."},
            {"C# Backend", "This is an introductory programming course using the C# language. It does not assume any prior programming experience. This course will prepare students for intermediate C# and ASP.NET courses."},
            {"Cloud Security", "FIXME"},
            {"Computer Security Analyst", "The security analyst plays a vital role in keeping an organization’s proprietary and sensitive information secure. He/she works inter-departmentally to identify and correct flaws in the company’s security systems, solutions, and programs while recommending specific measures that can improve the company’s overall security posture."},
            {"Cybersecurity", "FIXME"},
            {"Digitization of the Legal Sector", "FIXME"},
            {"Embedded Dev", "FIXME"},
            {"Self-Paced Programs", "Self-paced learning enables employees to create their schedules. It is especially helpful for participants that have other tasks and can’t attend a live class or training. Even if there is a deadline to complete a course, for example, they can choose how and when they take it."},
            {"Web Design", "FIXME"}
        };

        private const int ModuleCapacity = 20;
        private static IEnumerable<string> ModuleNamePool = new List<string>(ModuleCapacity)
        {
            "Android Module 1",
            "Android Module 2",
            
            "Big Data Module 1",
            "Big Data Module 2",
            
            "C# Backend Module 1",
            "C# Backend Module 2",
            
            "Cloud Security Module 1",
            "Cloud Security Module 2",
            
            "Computer Security Analyst Module 1",
            "Computer Security Analyst Module 2",
            
            "Cybersecurity Module 1",
            "Cybersecurity Module 2",
            
            "Digitization of the Legal Sector Module 1",
            "Digitization of the Legal Sector Module 2",
            
            "Embedded Dev Module 1",
            "Embedded Dev Module 2",
            
            "Big Data Module 1",
            "Big Data Module 2",
            
            "Self-Paced Programs Module 1",
            "Self-Paced Programs Module 2",
            
            "Web Design Module 1",
            "Web Design Module 2"
        };

        public static async Task InitAsync(LmsDbContext context, IServiceProvider services)
        {
            if (await context.Courses.AnyAsync()) return;

            var courses = GetCourses();
            var modules = GetModules(courses.Result);

            await context.Courses.AddRangeAsync(courses.Result);
            await context.Modules.AddRangeAsync(modules.Result);
            await context.SaveChangesAsync();
        }


        private static async Task<IEnumerable<Course>> GetCourses()
        {
            var courses = new List<Course>();

            for (var i = 0; i < CourseCapacity; i++)
            {
                var (name, desciption) = CourseNamePool.ElementAt(i);
                var date = DateTime.Now.AddDays(_random.Next(10));
                var course = new Course
                {
                    Name = name,
                    Description = desciption,
                    StartDate = date,
                    EndDate = date.AddMonths(6)
                };
                courses.Add(course);
            }
            return await Task.FromResult(courses);
        }
        
        private static async Task<IEnumerable<Module>> GetModules(IEnumerable<Course> courses) // course
        {
            var modules = new List<Module>();

            for (var i = 0; i < ModuleCapacity; i++)
            {
                var date = DateTime.Now.AddDays(_random.Next(10));
                var module = new Module
                {
                    Name = "Name",
                    Description = "Desc",
                    StartDate = date,
                    EndDate = date.AddMonths(6),
                    
                };
                modules.Add(module);
            }

            var index = 0;
            foreach (var course in courses)
            {
                course.Modules = new List<Module>() { modules.ElementAt(index), modules.ElementAt(index + 1)};
                index += 2;
            }

            return await Task.FromResult(modules);
        }
        
        // private static List<Course> GetCourses()
        // {
        //     return new List<Course>
        //     {
        //         new Course { Name = "C# Fundamentals", StartDate = new DateTime(2021, 06, 14), EndDate=new DateTime(2021, 06, 14).AddMonths(6)},
        //         new Course { Name = "Frontend", StartDate = new DateTime(2021, 07, 15), EndDate= new DateTime(2021, 07, 15).AddMonths(6) },
        //         new Course { Name = "MVC", StartDate = new DateTime(2021, 07, 15), EndDate= new DateTime(2021, 07, 15).AddMonths(6) },
        //         new Course { Name = "Database", StartDate = new DateTime(2021, 08, 30), EndDate= new DateTime(2021, 08, 30).AddMonths(6) },
        //         new Course { Name = "API", StartDate = new DateTime(2021, 09, 08), EndDate=new DateTime(2021, 09, 08).AddMonths(6) },
        //         new Course { Name = "Backend", StartDate = new DateTime(2021, 09, 20), EndDate=new DateTime(2021, 09, 20).AddMonths(6) },
        //     };
        // }
        //
        // private static List<Module> GetModules()
        // {
        //     return new List<Module>
        //     {
        //         new Module { CourseId = 1, Name = "C# Intro", StartDate = new DateTime(2021, 06, 14) },
        //         new Module { CourseId = 1, Name = "OOP", StartDate = new DateTime(2021, 06, 22) },
        //         new Module { CourseId = 1, Name = "Generics", StartDate = new DateTime(2021, 06, 29) },
        //         new Module { CourseId = 1, Name = "Delegates", StartDate = new DateTime(2021, 07, 02) },
        //         new Module { CourseId = 2, Name = "HTML", StartDate = new DateTime(2021, 07, 15) },
        //         new Module { CourseId = 2, Name = "CSS", StartDate = new DateTime(2021, 07, 16) },
        //         new Module { CourseId = 2, Name = "Bootstrap", StartDate = new DateTime(2021, 08, 03) },
        //         new Module { CourseId = 2, Name = "JavaScript", StartDate = new DateTime(2021, 08, 06) },
        //         new Module { CourseId = 3, Name = "MVC Intro", StartDate = new DateTime(2021, 08, 12) },
        //         new Module { CourseId = 3, Name = "PartialView/ViewModel", StartDate = new DateTime(2021, 08, 19) },
        //         new Module { CourseId = 3, Name = "Git", StartDate = new DateTime(2021, 08, 20) },
        //         new Module { CourseId = 4, Name = "SQL Bolt", StartDate = new DateTime(2021, 08, 30) },
        //         new Module { CourseId = 4, Name = "EntityFramework", StartDate = new DateTime(2021, 08, 31) },
        //         new Module { CourseId = 5, Name = "API Intro", StartDate = new DateTime(2021, 09, 08) },
        //         new Module { CourseId = 5, Name = "RESTful APIs", StartDate = new DateTime(2021, 09, 10) },
        //     };
        // }


    }
}
