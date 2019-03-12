namespace P01_StudentSystem
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using P01_StudentSystem.Data;
    using System;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            LoggerFactory SqlCommandLoggerFactory = new LoggerFactory(
                new[] {
                new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information, true) });

            var serviceCollection = new ServiceCollection();

            var serviceProvider = serviceCollection
                .AddDbContext<StudentSystemContext>(options =>
                {
                    options.UseSqlServer("Server=localhost;Database=StudentSystem;Integrated Security=True",
                s => s.MigrationsAssembly("P01_StudentSystem.Data"))
                .UseLoggerFactory(SqlCommandLoggerFactory)
                .EnableSensitiveDataLogging();
                })
                .BuildServiceProvider();

            var context = serviceProvider.GetService<StudentSystemContext>();

        }
    }
}
