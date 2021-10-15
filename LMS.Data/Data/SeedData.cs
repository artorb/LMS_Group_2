using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lms.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Lms.Data.Data
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SeedData
    {
        private static readonly Random _random = new Random(10);

        private static readonly Dictionary<string, string> CourseNamePool =
            new() /* Name - Description pair */
            {
                {
                    "Android Course",
                    "Expand your mobile app reach through this Android application development and programming training. Android's open source platform offers compatibility with a wide range of devices, which provide global access to the mobile market."
                },
                {
                    "Big Data",
                    "The course gives an overview of the Big Data phenomenon, focusing then on extracting value from the Big Data using predictive analytics techniques."
                },
                {
                    "C# Backend",
                    "This is an introductory programming course using the C# language. It does not assume any prior programming experience. This course will prepare students for intermediate C# and ASP.NET courses."
                },
                { "Cloud Security", "FIXME" },
                {
                    "Computer Security Analyst",
                    "The security analyst plays a vital role in keeping an organization’s proprietary and sensitive information secure. He/she works inter-departmentally to identify and correct flaws in the company’s security systems, solutions, and programs while recommending specific measures that can improve the company’s overall security posture."
                },
                { "Cybersecurity", "FIXME" },
                { "Digitization of the Legal Sector", "FIXME" },
                { "Embedded Dev", "FIXME" },
                {
                    "Self-Paced Programs",
                    "Self-paced learning enables employees to create their schedules. It is especially helpful for participants that have other tasks and can’t attend a live class or training. Even if there is a deadline to complete a course, for example, they can choose how and when they take it."
                },
                { "Web Design", "FIXME" }
            };

        private static readonly Dictionary<string, string> ModuleNamePool = new()
        {
            { "Android Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Android Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Big Data Module 1", "ModuleDescription3 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Big Data Module 2",
                "ModuleDescription4 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics and prescriptive analytics, and apply these concepts to propose solutions in Big Data cases." },

            { "C# Backend Module 1", "ModuleDescription5 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "C# Backend Module 2", "ModuleDescription6 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Cloud Security Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Cloud Security Module 2", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Computer Security Analyst Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Computer Security Analyst Module 2", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Cybersecurity Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Cybersecurity Module 2", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Digitization of the Legal Sector Module 1", "ModuleDescription1" },
            { "Digitization of the Legal Sector Module 2", "ModuleDescription1" },

            { "Embedded Dev Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Embedded Dev Module 2", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Self-Paced Programs Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Self-Paced Programs Module 2", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Web Design Module 1", "ModuleDescription111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111" },
            { "Web Design Module 2", "ModuleDescription1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111" }
        };

        private static readonly Dictionary<string, string> ActivityNamePool = new()
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

            { "Self-Paced Programs Activity 1", "ActivityDescription1" },
            { "Self-Paced Programs Activity 2", "ActivityDescription1" },

            { "Web Design Activity 1", "ActivityDescription1" },
            { "Web Design Activity 2", "ActivityDescription1" }
        };

        private static readonly List<string> StudentNamePool = new()
        {
            { "Lucas" },
            { "Liam" },
            { "William" },
            { "Elias" },
            { "Noah" },
            { "Hugo" },
            { "Oliver" },
            { "Oscar" },
            { "Adam" },
            { "Matt" },
            { "Lars" },
            { "Mikael" },
            { "Anders" },
            { "Erik" },
            { "Per" },
            { "Karl" },
            { "Peter" },
            { "Thomas" },
            { "Jan" },
            { "Ola" },
            { "Gustaf" },
            { "Sven" },
            { "Nils" },
            { "Alexander" },
            { "Vincent" },
            { "Theo" },
            { "Isak" },
            { "Arvid" },
            { "August" },
            { "Ludvig" }
        };

        private static UserManager<ApplicationUser> _userManager;

        private static LmsDbContext _context;

        public static async Task InitAsync(LmsDbContext context, IServiceProvider services)
        {
            // if (await context.Courses.AnyAsync()) return;
            _context = context;
            _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var activityTypes = GetActivityType().Result;
            await context.ActivityTypes.AddRangeAsync(activityTypes);

            var courses = GetCourses().Result;
            await context.Courses.AddRangeAsync(courses);

            var modules = GetModules(courses).Result;
            await context.Modules.AddRangeAsync(modules);

            var activities = GetActivities(modules, activityTypes).Result;
            await context.Activities.AddRangeAsync(activities);


            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var teacherRole = new IdentityRole { Name = "Teacher" };
            await roleManager.CreateAsync(teacherRole);

            var studentRole = new IdentityRole { Name = "Student" };
            await roleManager.CreateAsync(studentRole);

            var teachers = await InitTeachers();
            await AddToRolesAsync(teachers.ToList(), teacherRole);

            var students = await InitStudents(courses);
            await AddToRolesAsync(students.ToList(), studentRole);

            var documents = await GetDocuments();
            await context.Documents.AddRangeAsync(documents);

            await context.SaveChangesAsync();
        }

        private static async Task AddToRolesAsync(IEnumerable<ApplicationUser> users, IdentityRole role)
        {
            if (users == null) throw new NullReferenceException($"Users are null");

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name)) continue;
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    throw new Exception($"AddToRolesAsync error ${result.Errors}");
                }
            }
        }

        private static async Task<IEnumerable<ApplicationUser>> InitTeachers()
        {
            const string pass = "12";

            var teachers = new List<ApplicationUser>();

            var teacher_john = new ApplicationUser
            {
                Email = "john@LearningSite.se",
                UserName = "John",
                Name = "Teacher John"
            };

            var teacher_sanna = new ApplicationUser
            {
                Email = "sanna@LearningSite.se",
                UserName = "Sanna",
                Name = "Teacher Sanna"
            };

            teachers.Add(teacher_john);
            teachers.Add(teacher_sanna);
            await _userManager.CreateAsync(teacher_sanna, pass);
            await _userManager.CreateAsync(teacher_john, pass);

            return await Task.FromResult(teachers);
        }

        private static async Task<IEnumerable<ApplicationUser>> InitStudents(IEnumerable<Course> courses)
        {
            var users = new List<ApplicationUser>();
            const string pass = "12";

            foreach (var student in StudentNamePool.Select(name => new ApplicationUser
            {
                Email = $"{name}@LearningSite.se",
                UserName = $"{name}",
                Name = $"{name}",
                Course = courses.ElementAt(GetRangedIncludedRandom(to: 10)) // FIXME to a method
            }))
            {
                users.Add(student);
                await _userManager.CreateAsync(student, pass);
            }

            return await Task.FromResult(users);
        }


        private static async Task<IEnumerable<Course>> GetCourses()
        {
            var courses = new List<Course>();

            foreach (var (name, description) in CourseNamePool)
            {
                var date = DateTime.Now.AddDays(GetRangedIncludedRandom(5, 10));
                var course = new Course
                {
                    Name = name,
                    Description = description,
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


            foreach (var (name, description) in ModuleNamePool)
            {
                var date = DateTime.Now.AddDays(GetRangedIncludedRandom(5, 10));
                var module = new Module
                {
                    Name = name,
                    Description = description,
                    StartDate = date,
                    EndDate = date.AddMonths(6),
                };
                modules.Add(module);
            }

            var dict = new Dictionary<Module, Module>();
            for (var i = 0; i < modules.Count; i += 2)
            {
                dict.Add(modules.ElementAt(i), modules.ElementAt(i + 1));
            }

            for (var i = 0; i < courses.Count(); i++)
            {
                courses.ElementAt(i).Modules = new List<Module>() { dict.Keys.ElementAt(i), dict.Values.ElementAt(i) };
            }

            // var index = 0;
            // foreach (var course in courses)
            // {
            //     course.Modules = new List<Module>
            //     {
            //         modules.ElementAt(index), modules.ElementAt(index + 1)
            //     }; //Cause some disorder in the order of the modules.
            //     index += 2;
            // }

            return await Task.FromResult(modules);
        }


        private static async Task<IEnumerable<Activity>> GetActivities(IEnumerable<Module> modules,
            IEnumerable<ActivityType> activityTypes)
        {
            var activities = new List<Activity>();

            foreach (var (name, description) in ActivityNamePool)
            {
                var date = DateTime.Now.AddDays(GetRangedIncludedRandom(to: 10));
                var activity = new Activity
                {
                    Name = name,
                    Description = description,
                    StartDate = date,
                    EndDate = date.AddMonths(6), //för activity
                    Deadline = date.AddMonths(6), //för uppgift för activity
                    ActivityType = activityTypes.ElementAt(GetRangedIncludedRandom(to: 10) % 3)
                };
                activities.Add(activity);
            }

            var index = 0;
            foreach (var module in modules)
            {
                module.Activities = new List<Activity> { activities.ElementAt(index) };
                index += 1;
            } // Same disorder error

            return await Task.FromResult(activities);
        }

        private static async Task<IEnumerable<ActivityType>> GetActivityType()
        {
            var types = new List<ActivityType>
            {
                new() { TypeName = "Laboratory" },
                new() { TypeName = "Lecture" },
                new() { TypeName = "Assignment" }
            };
            return await Task.FromResult(types);
        }

        private static int GetRangedIncludedRandom(int from = 0, int to = 0)
        {
            var result = new Random();
            return result.Next(from, to);
        }


        private static async Task<IEnumerable<Document>> GetDocuments()
        {
            var types = new List<Document>
            {
                new() { Name = "Des1", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/modul1/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 1},
                new() { Name = "Des2", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/modul1/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 2 },
                new() { Name = "Des3", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 3},
                new() { Name = "Des4", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 4},
                new() { Name = "Des5", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 5},
                new() { Name = "Des6", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 6},
                new() { Name = "Des7", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 7},
                new() { Name = "Des8", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 8},
                new() { Name = "Des9", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 9},
                new() { Name = "Des10", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 10},
                new() { Name = "Des11", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 11},
                new() { Name = "Des12", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 12},
                new() { Name = "Des13", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 13},
                new() { Name = "Des14", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 14},
                new() { Name = "Des15", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 15},
                new() { Name = "Des16", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 16},
                new() { Name = "Des17", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 17},
                new() { Name = "Des18", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 18},
                new() { Name = "Des19", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 19},
                new() { Name = "Des20", Description="ModuleDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ModuleId= 20},

                new() { Name = "DesAct1", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 1},
                new() { Name = "DesAct2", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 2 },
                new() { Name = "DesAct3", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 3},
                new() { Name = "DesAct4", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 4},
                new() { Name = "DesAct5", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 5},
                new() { Name = "DesAct6", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 6},
                new() { Name = "DesAct7", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 7},
                new() { Name = "DesAct8", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 8},
                new() { Name = "DesAct9", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 9},
                new() { Name = "DesAct10", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 10},
                new() { Name = "DesAct11", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 11},
                new() { Name = "DesAct12", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 12},
                new() { Name = "DesAct13", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 13},
                new() { Name = "DesAct14", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 14},
                new() { Name = "DesAct15", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 15},
                new() { Name = "DesAct16", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 16},
                new() { Name = "DesAct17", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 17},
                new() { Name = "DesAct18", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 18},
                new() { Name = "DesAct19", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 19},
                new() { Name = "DesAct20", Description="ActivityDescription", UploadDate=DateTime.Now, HashName="teacher/Group2.pdf", Uploader="teacher@gmail.com", ActivityId= 20},


            };
            return await Task.FromResult(types);
        }











    }
}