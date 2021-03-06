using Lms.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data
{
    public class LmsDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public LmsDbContext(DbContextOptions<LmsDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Module> Modules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Course>()
                .HasMany<ApplicationUser>(b => b.Users)
                .WithOne(c => c.Course)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Course>()
         .HasMany<Module>(b => b.Modules)
         .WithOne(c => c.Course)
         .OnDelete(DeleteBehavior.SetNull);

        }
    }
}