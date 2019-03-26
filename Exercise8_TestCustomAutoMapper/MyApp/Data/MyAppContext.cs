namespace MyApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using MyApp.Models;


    public class MyAppContext : DbContext
    {
        public MyAppContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=MyApp;Integrated Security=true"
                    );
            }
        }

        public MyAppContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

    }
}
