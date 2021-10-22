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
    public class SeedData2
    {
        private static readonly Random _random = new(10);

        private static UserManager<ApplicationUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;
        private static LmsDbContext _context;

        public static async Task InitAsync(LmsDbContext context, IServiceProvider services)
        {
            _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            _context = context;

            var activityTypes = GetActivityType().Result;
            await _context.ActivityTypes.AddRangeAsync(activityTypes);

            var courses = GetCourses().Result;
            await _context.Courses.AddRangeAsync(courses);

            var modules = GetModules(courses).Result;
            await _context.Modules.AddRangeAsync(modules);

            var activities = GetActivities(modules, activityTypes).Result;
            await _context.Activities.AddRangeAsync(activities);

            var teacherRole = new IdentityRole { Name = "Teacher" };
            await _roleManager.CreateAsync(teacherRole);
            
            var studentRole = new IdentityRole { Name = "Student" };
            await _roleManager.CreateAsync(studentRole);

            var teachers = await InitTeachers();
            await AddToRolesAsync(teachers.ToList(), teacherRole);

            var students = await InitStudents(courses);
            await AddToRolesAsync(students.ToList(), studentRole);

            var documents = await GetDocuments(courses, modules, activities);
            await _context.Documents.AddRangeAsync(documents);

            await _context.SaveChangesAsync();
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



            foreach (var student in SeedDataInfo.StudentNamePool.Select(name => new ApplicationUser
            {
                Email = $"{name}@LearningSite.se",
                UserName = $"{name}",
                Name = $"{name}",
                Course = courses.ElementAt(GetRangedIncludedRandom(to: courses.Count())) // FIXME to a method
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

            foreach (var (name, description) in SeedDataInfo.CourseNamePool)
            {
                var date = DateTime.Now.AddDays(GetRangedIncludedRandom(-10, -5));
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

            int count = 0;
            foreach (var course in courses)
            {
                course.Modules = new List<Module>();

                var dayDifference = (course.EndDate - course.StartDate).TotalDays / 2;

                for (int i = count; i < count + 2; i++)
                {
                    var (name, description) = SeedDataInfo.ModuleNamePool.ElementAt(i);
                    var module = new Module
                    {
                        Name = name,
                        Description = description,
                        StartDate = i % 2 == 0 ? course.StartDate : course.StartDate.AddDays(dayDifference),
                        EndDate = i % 2 == 0 ? course.StartDate.AddDays(dayDifference) : course.EndDate
                    };
                    course.Modules.Add(module);
                    modules.Add(module);
                }
                count += 2;
            }
            return await Task.FromResult(modules);
        }


        private static async Task<IEnumerable<Activity>> GetActivities(IEnumerable<Module> modules,
            IEnumerable<ActivityType> activityTypes)
        {
            var activities = new List<Activity>();

            int count = 0;
            foreach (var module in modules)
            {
                module.Activities = new List<Activity>();

                var dayDifference = (module.EndDate - module.StartDate).TotalDays / 2;

                for (int i = count; i < count + 2; i++)
                {
                    var (name, description) = SeedDataInfo.ActivityNamePool.ElementAt(i);
                    var activity = new Activity
                    {
                        Name = name,
                        Description = description,
                        StartDate = i % 2 == 0 ? module.StartDate : module.StartDate.AddDays(dayDifference),
                        EndDate = i % 2 == 0 ? module.StartDate.AddDays(dayDifference) : module.EndDate,
                        Deadline = i % 2 == 0 ? module.StartDate.AddDays(dayDifference) : module.EndDate,
                        ActivityType = activityTypes.ElementAt(GetRangedIncludedRandom(to: activityTypes.Count()))
                    };
                    module.Activities.Add(activity);
                    activities.Add(activity);
                }
                count += 2;
            }
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
            return _random.Next(from, to);
        }


        private static async Task<IEnumerable<Document>> GetDocuments(IEnumerable<Course> courses,
            IEnumerable<Module> modules, IEnumerable<Activity> activities)
        {

            var paths = new List<string>();
            foreach (var module in modules)
            {
                foreach (var activity in module.Activities)
                {
                    var fileName1 = $"{activity.Name}" + ".doc";
                    var fileName2 = $"{module.Name}" + ".doc";
                    var fileName3 = $"{module.Course.Name}" + ".doc";

                    if (module.Name == activity.Module.Name)
                    {
                        Directory.CreateDirectory($"wwwroot/Uploads/{module.Course.Name}/{activity.Module.Name}/{activity.Name}");

                        var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Uploads/{module.Course.Name}/{module.Name}/{activity.Name}", fileName1);
                        var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Uploads/{module.Course.Name}/{module.Name}", fileName2);
                        var filePath3 = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Uploads/{module.Course.Name}", fileName3);

                        paths.Add(filePath1);
                        paths.Add(filePath2);
                        paths.Add(filePath3);

                        using (var fs = new FileStream(filePath1, FileMode.Create))
                        {
                            using (var sw = new StreamWriter(fs))
                                sw.WriteLine($"{activity.Name} \n\nThis activity contains ...");
                        }

                        using (var fs = new FileStream(filePath2, FileMode.Create))
                        {
                            using (var sw = new StreamWriter(fs))
                                sw.WriteLine($"{module.Name} \n\nThis module contains ...");
                        }

                        using (var fs = new FileStream(filePath3, FileMode.Create))
                        {
                            using (var sw = new StreamWriter(fs))
                                sw.WriteLine($"Welcome in the {module.Course.Name} course!");
                        }
                    }
                }
            }

            var documents = courses.Select(course => new Document
            {
                Name = $"{course.Name} document",
                Description = $"{course.Description}",
                UploadDate = course.StartDate,
                HashName = $"{course.Name}/{course.Name}.doc",
                Uploader = "john@LearningSite.se",
                Course = course
            }).ToList();


            documents.AddRange(modules.Select(module => new Document
            {
                Name = $"{module.Name} document",
                Description = $"{module.Description}",
                UploadDate = module.StartDate,
                HashName = $"{module.Course.Name}/{module.Name}/{module.Name}.doc",
                Uploader = "john@LearningSite.se",
                Module = module
            }));


            /* Activity documents seed */
            documents.AddRange(activities.Select(activity => new Document
            {
                Name = $"{activity.Name} document",
                Description = $"{activity.Description}",
                UploadDate = activity.StartDate,
                HashName = $"{activity.Module.Course.Name}/{activity.Module.Name}/{activity.Name}/{activity.Name}.doc",
                Uploader = "john@LearningSite.se",
                Activity = activity
            }));

            return await Task.FromResult(documents);
        }
    }
}