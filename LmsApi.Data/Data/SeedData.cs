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

            await context.Categories.AddRangeAsync(GetCategories());
            await context.Subjects.AddRangeAsync(GetSubjects());
            await context.Levels.AddRangeAsync(GetLevels());
            await context.SaveChangesAsync();

            // They're not needed since literatures are generated in GetAuthors
            //await context.Literatures.AddRangeAsync(GetLiteratures());
            //await context.SaveChangesAsync();
            var literatures = GetLiteratures();
            await context.Authors.AddRangeAsync(GetAuthors(literatures));
            await context.SaveChangesAsync();
        }

        private static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category { Name = "Books" },
                new Category { Name = "Articles" },
                new Category { Name = "Documentations" },
                new Category { Name = "Blog posts" },
                new Category { Name = "Learning" },
                new Category { Name = "Uncategorized" },
            };
        }

        private static List<Subject> GetSubjects()
        {
            return new List<Subject>
            {
                new Subject { Name = "IT" },
                new Subject { Name = "Software Development" },
                new Subject { Name = "C#" },
                new Subject { Name = "Frontend" },
                new Subject { Name = "ASP.NET Core" },
                new Subject { Name = "Databases" },
                new Subject { Name = "API" },
                new Subject { Name = "Cloud" },
                new Subject { Name = "Test" },
                new Subject { Name = "Project Management" },
                new Subject { Name = "Support" },
                new Subject { Name = "Economics" },
                new Subject { Name = "Sales" },
                new Subject { Name = "Marketing" }
            };
        }

        private static List<Level> GetLevels()
        {
            return new List<Level>
            {
                new Level { Name = "Beginner" },
                new Level { Name = "Intermediate" },
                new Level { Name = "Advanced" },
                new Level { Name = "Unspecified" }
            };
        }

        private static List<Author> GetAuthors(List<Literature> literatures)
        {
            return new List<Author>
            {
                new Author { FirstName = "Tom", LastName = "Dykstra", Literatures = GetRandomListContent(literatures) },
                new Author { FirstName = "Wade", LastName = "Pickett", Literatures = GetRandomListContent(literatures) },
                new Author { FirstName = "Rick", LastName = "Anderson", Literatures = GetRandomListContent(literatures) },
                new Author { FirstName = "Kirk", LastName = "Larkin", Literatures = GetRandomListContent(literatures) },
                new Author { FirstName = "Steve", LastName = "Jobs", Literatures = GetRandomListContent(literatures) },
                new Author { FirstName = "Bill", LastName = "Gates", Literatures = GetRandomListContent(literatures) },
                new Author { FirstName = "Elon", LastName = "Musk", Literatures = GetRandomListContent(literatures) },
                new Author { FirstName = "Jeff", LastName = "Bezos", Literatures = GetRandomListContent(literatures) }
            };
        }

        private static List<Literature> GetLiteratures()
        {
            return new List<Literature>
            {
                new Literature
                {
                    Title = "Implementing the Repository and Unit of Work Patterns in an ASP.NET MVC Application",
                    Description = "Microsoft Docs...",
                    PublishDate = new DateTime(2021, 03, 05),
                    CategoryId = 3,
                    SubjectId = 4,
                    LevelId = 2,
                },
                new Literature
                {
                    Title = "Web API with ASP.NET Core",
                    Description = "Microsoft Docs...",
                    PublishDate = new DateTime(2021, 10, 07),
                    CategoryId = 3,
                    SubjectId = 2,
                    LevelId = 1,
                },
                new Literature
                {
                    Title = "Literature 1",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 03, 05),
                    CategoryId = 1,
                    SubjectId = 1,
                    LevelId = 1,
                },
                new Literature
                {
                    Title = "Literature 2",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 10, 07),
                    CategoryId = 2,
                    SubjectId = 2,
                    LevelId = 2,
                },
                new Literature
                {
                    Title = "Literature 3",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 03, 05),
                    CategoryId = 4,
                    SubjectId = 5,
                    LevelId = 3,
                },
                new Literature
                {
                    Title = "Literature 4",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 10, 07),
                    CategoryId = 4,
                    SubjectId = 2,
                    LevelId = 1,
                },
                new Literature
                {
                    Title = "Literature 5",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 03, 05),
                    CategoryId = 5,
                    SubjectId = 4,
                    LevelId = 2,
                },
                new Literature
                {
                    Title = "Literature 6",
                    Description = "This is the desc...",
                    PublishDate = new DateTime(2021, 10, 07),
                    CategoryId = 4,
                    SubjectId = 1,
                    LevelId = 1,
                }
            };
        }

        // Add random literatures to an author
        private static List<Literature> GetRandomListContent(List<Literature> literatures)
        {
            var random = new Random();
            int randomIndex;

            int literatureCount = literatures.Count;
            var randomLiteratures = new List<Literature>();
            int randomLoops = random.Next(1, literatureCount);
            int counter = 0;
            while (counter < randomLoops)
            {
                randomIndex = random.Next(0, literatureCount);
                randomLiteratures.Add(literatures.ElementAtOrDefault(randomIndex));
                counter++;
            }
            return randomLiteratures;
        }

    }
}
