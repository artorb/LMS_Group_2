using Lms.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private const int ActivityCapacity = 20;
        private static readonly Dictionary<string, string> CourseNamePool = new(CourseCapacity) /* Name - Description pair */
        {
            { "Android Course", "Expand your mobile app reach through this Android application development and programming training. Android's open source platform offers compatibility with a wide range of devices, which provide global access to the mobile market." },
            { "Big Data", "The course gives an overview of the Big Data phenomenon, focusing then on extracting value from the Big Data using predictive analytics techniques." },
            { "C# Backend", "This is an introductory programming course using the C# language. It does not assume any prior programming experience. This course will prepare students for intermediate C# and ASP.NET courses." },
            { "Cloud Security", "FIXME" },
            { "Computer Security Analyst", "The security analyst plays a vital role in keeping an organization’s proprietary and sensitive information secure. He/she works inter-departmentally to identify and correct flaws in the company’s security systems, solutions, and programs while recommending specific measures that can improve the company’s overall security posture." },
            { "Cybersecurity", "FIXME" },
            { "Digitization of the Legal Sector", "FIXME" },
            { "Embedded Dev", "FIXME" },
            { "Self-Paced Programs", "Self-paced learning enables employees to create their schedules. It is especially helpful for participants that have other tasks and can’t attend a live class or training. Even if there is a deadline to complete a course, for example, they can choose how and when they take it." },
            { "Web Design", "FIXME" }
        };

        private const int ModuleCapacity = 20;
        private static Dictionary<string, string> ModuleNamePool = new(ModuleCapacity)
        {
            { "Android Module 1", "ModuleDescription1" },
            { "Android Module 2", "ModuleDescription2" },

            { "Big Data Module 1", "ModuleDescription3" },
            { "Big Data Module 2", "ModuleDescription4" },

            { "C# Backend Module 1", "ModuleDescription5" },
            { "C# Backend Module 2", "ModuleDescription6" },

            { "Cloud Security Module 1", "ModuleDescription1" },
            { "Cloud Security Module 2", "ModuleDescription1" },

            { "Computer Security Analyst Module 1", "ModuleDescription1" },
            { "Computer Security Analyst Module 2", "ModuleDescription1" },

            { "Cybersecurity Module 1", "ModuleDescription1" },
            { "Cybersecurity Module 2", "ModuleDescription1" },

            { "Digitization of the Legal Sector Module 1", "ModuleDescription1" },
            { "Digitization of the Legal Sector Module 2", "ModuleDescription1" },

            { "Embedded Dev Module 1", "ModuleDescription1" },
            { "Embedded Dev Module 2", "ModuleDescription1" },

            { "New Big Data Module 1", "ModuleDescription1" },
            { "New Big Data Module 2", "ModuleDescription1" },

            { "Self-Paced Programs Module 1", "ModuleDescription1" },
            { "Self-Paced Programs Module 2", "ModuleDescription1" },

            { "Web Design Module 1", "ModuleDescription1" },
            { "Web Design Module 2", "ModuleDescription1" }
        };




        private static Dictionary<string, string> ActivityNamePool = new(ActivityCapacity)
        {
            { "Android Activity 1", "ActivityDescription1" },
            { "Android Activity 2", "ActivityDescription1" },

            { "Big Data Activity 1", "ActivityDescription1" },
            { "Big Data Activity 2", "ActivityDescription1" },

            { "C# Backend Activity 1", "ActivityDescription1" },
            { "C# Backend Activity 2", "ActivityDescription1" },

            { "Cloud Security Activity 1", "ActivityDescription1" },
            { "Cloud Security Activity 2", "ActivityDescription1" },

            { "Computer Security Analyst Activity 1", "ActivityDescription1" },
            { "Computer Security Analyst Activity 2", "ActivityDescription1" },

            { "Cybersecurity Activity 1", "ActivityDescription1" },
            { "Cybersecurity Activity 2", "ActivityDescription1" },

            { "Digitization of the Legal Sector Activity 1", "ActivityDescription1" },
            { "Digitization of the Legal Sector Activity 2", "ActivityDescription1" },

            { "Embedded Dev Activity 1", "ActivityDescription1" },
            { "Embedded Dev Activity 2", "ActivityDescription1" },

            { "New Big Data Activity 1", "ActivityDescription1" },
            { "New Big Data Activity 2", "ActivityDescription1" },

            { "Self-Paced Programs Activity 1", "ActivityDescription1" },
            { "Self-Paced Programs Activity 2", "ActivityDescription1" },

            { "Web Design Activity 1", "ActivityDescription1" },
            { "Web Design Activity 2", "ActivityDescription1" }
        };







        public static async Task InitAsync(LmsDbContext context, IServiceProvider services)
        {
            // if (await context.Courses.AnyAsync()) return;

            var activityTypes = GetActivityType();
            var courses = GetCourses();
            var modules = GetModules(courses.Result);
            var activities = GetActivities(modules.Result, activityTypes);

            await context.Courses.AddRangeAsync(courses.Result);
            await context.Modules.AddRangeAsync(modules.Result);
            await context.Activities.AddRangeAsync(activities.Result);

            //using (var db = services.GetRequiredService<LmsDbContext>())
            //{
            const string passWord = "AdminNet21!";
            const string roleName = "Teacher";

            const string passWordStudent = "StudentNet21!";
            const string roleNameStudent = "Student";

            var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var role = new IdentityRole { Name = roleName };
            await roleManager.CreateAsync(role);

            var studentRole = new IdentityRole { Name = roleNameStudent };
            await roleManager.CreateAsync(studentRole);

            var users = GetUsers();
            await context.AddRangeAsync(users);

            foreach (var item in users)
            {
                var result = await userManager.CreateAsync(item, passWord);
                if (!result.Succeeded) throw new Exception(String.Join("\n", result.Errors));
                await userManager.AddToRoleAsync(item, "Teacher");
            }

            var students = GetStudent();
            await context.AddRangeAsync(students);

            foreach (var item in students)
            {
                var result = await userManager.CreateAsync(item, passWordStudent);
                if (!result.Succeeded) throw new Exception(String.Join("\n", result.Errors));
                await userManager.AddToRoleAsync(item, "Student");
            }
            await context.SaveChangesAsync();
        }


        private static List<ApplicationUser> GetUsers()
        {
            var users = new List<ApplicationUser>();
            var appUser = new ApplicationUser
            {
                Email = "admin@LearningSite.se",
                UserName = "admin@LearningSite.se",
                Name = "AdminNname"
            };
            users.Add(appUser);

            return users;
        }

        private static List<ApplicationUser> GetStudent()
        {
            var users = new List<ApplicationUser>();
            var appUser = new ApplicationUser
            {
                Email = "student@LearningSite.se",
                UserName = "student@LearningSite.se",
                Name = "StudentName",
                CourseId = 2
            };
            users.Add(appUser);

            return users;
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
                var (name, desciption) = ModuleNamePool.ElementAt(i);
                var date = DateTime.Now.AddDays(_random.Next(10));
                var module = new Module
                {
                    Name = name,
                    Description = desciption,
                    StartDate = date,
                    EndDate = date.AddMonths(6),

                };
                modules.Add(module);
            }

            var index = 0;
            foreach (var course in courses)
            {
                course.Modules = new List<Module>() { modules.ElementAt(index), modules.ElementAt(index + 1) };
                index += 2;
            }

            return await Task.FromResult(modules);
        }



        private static async Task<IEnumerable<Activity>> GetActivities(IEnumerable<Module> modules, IEnumerable<ActivityType> activityTypes) // module
        {
            var activities = new List<Activity>();

            for (var i = 0; i < ActivityCapacity; i++)
            {
                var (name, desciption) = ActivityNamePool.ElementAt(i);
                var date = DateTime.Now.AddDays(_random.Next(10));
                var activity = new Activity
                {
                    Name = name,
                    Description = desciption,
                    StartDate = date,
                    EndDate = date.AddMonths(6), //för activity
                    Deadline = date.AddMonths(6), //för uppgift för activity
                    ActivityType = activityTypes.ElementAt(_random.Next(10) % 3)
                };
                activities.Add(activity);
            }

            var index = 0;
            foreach (var module in modules)
            {
                module.Activities = new List<Activity>() { activities.ElementAt(index) };
                index += 1;
            }

            return await Task.FromResult(activities);
        }


        private static List<ActivityType> GetActivityType()
        {
            return new List<ActivityType>
            {
                new ActivityType { TypeName = "Laboratory" },
                new ActivityType { TypeName = "Lecture" },
                new ActivityType { TypeName = "Assignment" }
            };
        }

























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
