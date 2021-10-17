using System;
using System.Collections.Generic;
using System.IO;
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
                { 
                    "Cloud Security", 
                    "The course leverages cloud computing security guidelines set forth by the International Organization for Standardization (ISO), National Institute of Standards and Technology (NIST), European Union Agency for Network and Information Security (ENISA), and Cloud Security Alliance (CSA). This course reviews security characteristics of leading cloud infrastructure providers and applied deployment scenarios with the internet of things (IoT) and blockchain." 
                },
                {
                    "Computer Security Analyst",
                    "The security analyst plays a vital role in keeping an organization’s proprietary and sensitive information secure. He/she works inter-departmentally to identify and correct flaws in the company’s security systems, solutions, and programs while recommending specific measures that can improve the company’s overall security posture."
                },
                { 
                    "Cybersecurity", 
                    "This introductory certification course is the fastest way to get up to speed in information security. Written and taught by battle-scarred security veterans, this entry-level course covers a broad spectrum of security topics and is liberally sprinkled with real life examples. A balanced mix of technical and managerial issues makes this course appealing to attendees who need to understand the salient facets of information security basics and the basics of risk management.  " 
                },
                { 
                    "Digitization of the Legal Sector",
                    "During the 13-month course, participants will be exposed to the expertise of both schools, which are leaders in their respective disciplines, namely, technology and law. And after completing the course, participants will be equipped with the knowledge and skills they need to make a difference in the field of technology law."
                },
                { 
                    "Embedded Dev",
                    "Welcome to the Introduction to Embedded Systems Software and Development Environments. This course is focused on giving you real world coding experience and hands on project work with ARM based Microcontrollers. You will learn how to implement software configuration management and develop embedded software applications. Course assignments include creating a build system using the GNU Toolchain GCC, using Git version control, and developing software in Linux on a Virtual Machine. The course concludes with a project where you will create your own build system and firmware that can manipulate memory." 
                },
                {
                    "Self-Paced Programs",
                    "Self-paced learning enables employees to create their schedules. It is especially helpful for participants that have other tasks and can’t attend a live class or training. Even if there is a deadline to complete a course, for example, they can choose how and when they take it."
                },
                { 
                    "Web Design", 
                    "This Specialization covers how to write syntactically correct HTML5 and CSS3, and how to create interactive web experiences with JavaScript. Mastering this range of technologies will allow you to develop high quality web sites that, work seamlessly on mobile, tablet, and large screen browsers accessible. During the capstone you will develop a professional-quality web portfolio demonstrating your growth as a web developer and your knowledge of accessible web design. This will include your ability to design and implement a responsive site that utilizes tools to create a site that is accessible to a wide audience, including those with visual, audial, physical, and cognitive impairments." 
                }
            };

        private static readonly Dictionary<string, string> ModuleNamePool = new()
        {
            { "Android Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Android Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Big Data Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Big Data Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics and prescriptive analytics, and apply these concepts to propose solutions in Big Data cases." },

            { "C# Backend Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "C# Backend Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Cloud Security Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Cloud Security Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Computer Security Analyst Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Computer Security Analyst Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Cybersecurity Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Cybersecurity Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Digitization of the Legal Sector Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Digitization of the Legal Sector Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Embedded Dev Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Embedded Dev Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Self-Paced Programs Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Self-Paced Programs Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Web Design Module 1", "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Web Design Module 2", "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " }
        };

        private static readonly Dictionary<string, string> ActivityNamePool = new()
        {
            { "Android Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Android Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Big Data Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Big Data Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "C# Backend Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "C# Backend Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Cloud Security Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Cloud Security Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Computer Security Analyst Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Computer Security Analyst Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },           

            { "Cybersecurity Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Cybersecurity Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Digitization of the Legal Sector Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Digitization of the Legal Sector Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Embedded Dev Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Embedded Dev Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Self-Paced Programs Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Self-Paced Programs Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },

            { "Web Design Activity 1", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
            { "Web Design Activity 2", "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics " },
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

            var documents = await GetDocuments(courses, modules, activities);
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


        private static async Task<IEnumerable<Document>> GetDocuments(IEnumerable<Course> courses, IEnumerable<Module> modules, IEnumerable<Activity> activities)
        {

            //creates directories in computer: course/module/activity
            //foreach (var module in modules)
            //{               
            //        Directory.CreateDirectory($"wwwroot/Uploads/{module.Course.Name}/{module.Name}");                
            //}

            
            //creating folders for teachers (course/module/activity)
            foreach (var module in modules)
            {
                foreach (var activity in activities)
                {
                    string fileName = $"{activity.Name}"+".txt";

                    if (!Directory.Exists($"wwwroot/Uploads/{module.Course.Name}/{module.Name}"))
                    {                       
                        if (module.Name == activity.Module.Name) {
                        Directory.CreateDirectory($"wwwroot/Uploads/{module.Course.Name}/{activity.Module.Name}/{activity.Name}");                           
                            break;
                        }
                    }
                }
            }

            //creates seed in database but not in the computer *************************************
            var documents = courses.Select(course => new Document
            {
                Name = $"{course.Name} document",
                Description = $"{course.Description}",
                UploadDate = course.StartDate,
                HashName = $"{course.Name}/{course.Name}.pdf",
                Uploader = "john@LearningSite.se",
                Course = course
            }).ToList();


            documents.AddRange(modules.Select(module => new Document
            {
                Name = $"{module.Name} document",
                Description = $"{module.Description}",
                UploadDate = module.StartDate,
                HashName = $"{module.Course.Name}/{module.Name}/{module.Name}.pdf",
                Uploader = "john@LearningSite.se",
                Module = module
            }));


            /* Activity documents seed */
            documents.AddRange(activities.Select(activity => new Document
            {
                Name = $"{activity.Name} document",
                Description = $"{activity.Description}",
                UploadDate = activity.StartDate,
                HashName = $"{activity.Module.Course.Name}/{activity.Module.Name}/{activity.Name}/{activity.Name}.pdf",
                Uploader = "john@LearningSite.se",
                Activity = activity
            }));

  

            return await Task.FromResult(documents);
            
        }











    }
}