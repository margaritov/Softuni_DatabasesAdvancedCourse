namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
                .Entity<Student>()
                .HasKey(s => s.StudentId);

        }

        public DbSet<Student> Students { get; set; }

    }

}
