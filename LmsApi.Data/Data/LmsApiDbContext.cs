using LmsApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Data.Data
{
    public class LmsApiDbContext : DbContext
    {
        public DbSet<Literature> Literatures { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Author> Authors { get; set; }
        
        public LmsApiDbContext (DbContextOptions<LmsApiDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
