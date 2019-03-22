namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    { 

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public StudentSystemContext()
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(Config.connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnConfiguringStudent(modelBuilder);

            OnConfgiuringCourse(modelBuilder);

            OnConfgiuringResource(modelBuilder);

            OnConfiguringHomework(modelBuilder);

            OnConfiguringStudentCourse(modelBuilder);
        }

        private void OnConfiguringStudentCourse(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(e => e.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(e => e.Course)
                .WithMany(c => c.StudentsEnrolled)
                .HasForeignKey(e => e.CourseId);
        }

        private void OnConfiguringHomework(ModelBuilder modelBuilder)
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

        private void OnConfgiuringResource(ModelBuilder modelBuilder)
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

            modelBuilder
                .Entity<Resource>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Resources);
        }

        private void OnConfgiuringCourse(ModelBuilder modelBuilder)
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

        private void OnConfiguringStudent(ModelBuilder modelBuilder)
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
