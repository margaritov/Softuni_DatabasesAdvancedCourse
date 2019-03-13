namespace P01_StudentSystem.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    {

        DbSet<Student> Students { get; set; }

        DbSet<Course> Courses { get; set; }

        DbSet<Resource> Resources { get; set; }

        DbSet<Homework> HomeworkSubmissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureStudentEntity(modelBuilder);

            ConfgiureCourseEntity(modelBuilder);

            ConfigureResourceEntity(modelBuilder);

            ConfigureHomeworkEntity(modelBuilder);

            ConfigureStudentCourseEntity(modelBuilder);
        }

        private void ConfigureStudentCourseEntity(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }

        private void ConfigureHomeworkEntity(ModelBuilder modelBuilder)
        {
            modelBuilder                           
                .Entity<Homework>()                           
                .HasKey(c => c.HomeworkId);

            modelBuilder
                .Entity<Homework>()
                .Property(h => h.Content)
                .IsUnicode(false);

            modelBuilder
                .Entity<Homework>()
                .HasOne<Student>(h => h.Student)
                .WithMany(s => s.HomeworkSubmissions);

            modelBuilder
                .Entity<Homework>()
                .HasOne<Course>(h => h.Course)
                .WithMany(c => c.HomeworkSubmissions);    
            //todo content type - enum and submission time?


        }

        private void ConfigureResourceEntity(ModelBuilder modelBuilder)
        {
            modelBuilder                           
                .Entity<Resource>()                            
                .HasKey(r => r.ResourceId);

            modelBuilder
                .Entity<Resource>()
                .Property(r => r.Name)
                .HasMaxLength(50);

            modelBuilder
               .Entity<Resource>()
               .Property(r => r.Url)
               .IsUnicode(false);

            modelBuilder
               .Entity<Resource>()
               .Property(r => r.Url)
               .IsUnicode(false);
        }

        private void ConfgiureCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Course>()
                .HasKey(c => c.CourseId);

            modelBuilder
                .Entity<Course>()
                .Property(c => c.Name)
                .HasMaxLength(80);

            modelBuilder
               .Entity<Course>()
               .Property(c => c.Description)
               .IsRequired(false);
        }

        private void ConfigureStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder
                .Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder
                .Entity<Student>()
                .Property(s => s.PhoneNumber)
                .HasMaxLength(10)
                .IsRequired(false)
                .IsUnicode(false)
                .IsFixedLength(true);

            modelBuilder
                .Entity<Student>()
                .Property(s => s.Birthday)
                .IsRequired(false);
        }
    }
}
