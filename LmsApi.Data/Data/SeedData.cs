using LmsApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApi.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(LmsApiDbContext context)
        {
            if (await context.Literatures.AnyAsync()) return;

            await context.Subjects.AddRangeAsync(GetSubjects());
            await context.Levels.AddRangeAsync(GetLevels());
            await context.SaveChangesAsync();

            await context.Literatures.AddRangeAsync(GetLiteratures());
            await context.SaveChangesAsync();
        }

        private static List<Subject> GetSubjects()
        {
            return new List<Subject>
            {
                new Subject { Name = "C#" },
                new Subject { Name = "Frontend" },
                new Subject { Name = "ASP.NET Core" },
                new Subject { Name = "MVC" },
                new Subject { Name = "Database" },
                new Subject { Name = "API" },
                new Subject { Name = "Cloud" },
            };
        }

        private static List<Level> GetLevels()
        {
            return new List<Level>
            {
                new Level { Name = "Beginner" },
                new Level { Name = "Intermediate" },
                new Level { Name = "Advanced" },
            };
        }

        private static List<Author> GetAuthors()
        {
            return new List<Author>
            {
                new Author { FirstName = "Tom", LastName = "Dykstra"/*, Literatures = GetLiteratures()*/},
                new Author { FirstName = "Wade", LastName = "Pickett"},
                new Author { FirstName = "Rick", LastName = "Anderson"},
                new Author { FirstName = "Kirk", LastName = "Larkin"}
            };
        }

        private static List<Literature> GetLiteratures(/*int number*/)
        {
            return new List<Literature>
            {
                new Literature
                {
                    Title = "Implementing the Repository and Unit of Work Patterns in an ASP.NET MVC Application",
                    Description = "Microsoft Docs...",
                    PublishDate = new DateTime(2021, 03, 05),
                    SubjectId = 4,
                    LevelId = 2,
                },
                new Literature
                {
                    Title = "Web API with ASP.NET Core",
                    Description = "Microsoft Docs...",
                    PublishDate = new DateTime(2021, 10, 07),
                    SubjectId = 2,
                    LevelId = 1,
                },
                new Literature
                {
                    Title = "Literature 1",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 03, 05),
                    SubjectId = 4,
                    LevelId = 1,
                },
                new Literature
                {
                    Title = "Literature 2",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 10, 07),
                    SubjectId = 2,
                    LevelId = 2,
                },
                new Literature
                {
                    Title = "Literature 3",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 03, 05),
                    SubjectId = 5,
                    LevelId = 3,
                },
                new Literature
                {
                    Title = "Literature 4",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 10, 07),
                    SubjectId = 2,
                    LevelId = 1,
                },
                new Literature
                {
                    Title = "Literature 5",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 03, 05),
                    SubjectId = 4,
                    LevelId = 2,
                },
                new Literature
                {
                    Title = "Literature 6",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 10, 07),
                    SubjectId = 1,
                    LevelId = 1,
                }
            };
        }


    }
}
