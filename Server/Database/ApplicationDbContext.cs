using Microsoft.EntityFrameworkCore;
using UniversityWebApp.Shared;

namespace UniversityWebApp.Database
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
               .HasOne<Student>()
               .WithMany()
               .HasForeignKey("StudentId")
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
               .HasIndex("StudentId", "CourseId")
               .IsUnique();
        }
    }
}
