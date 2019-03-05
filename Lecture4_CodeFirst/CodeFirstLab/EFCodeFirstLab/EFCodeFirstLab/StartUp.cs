namespace EFCodeFirstLab
{
    using EFCodeFirst.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            LoggerFactory SqlCommandLoggerFactory
                   = new LoggerFactory(new[]
                   {
                    new ConsoleLoggerProvider((category,level)
                    => category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information, true)});

            string connectionString = "Server=localhost;Database=BlogDB;Integrated Security=True";

            DbContextOptionsBuilder<BlogDbContext> optionBuilder = new DbContextOptionsBuilder<BlogDbContext>();

            optionBuilder
                .UseSqlServer(connectionString, s => s.MigrationsAssembly("EFCodeFirst.Infrastructure"))
                .UseLoggerFactory(SqlCommandLoggerFactory)
                .EnableSensitiveDataLogging();

            using (var context = new BlogDbContext())
            {
                var user = context.Users.FirstOrDefault(); 
            }
        }
    }
}
