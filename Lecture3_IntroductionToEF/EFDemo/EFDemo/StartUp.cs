namespace EFDemo
{
    using EFDemoDBFirst.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using System;
    using System.Linq;

    public class StartUp
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<SoftUniContext> optionBuilder 
                = new DbContextOptionsBuilder<SoftUniContext>();

            LoggerFactory SqlCommandLoggerFactory
                = new LoggerFactory(new[]
                {
                    new ConsoleLoggerProvider((category, level)

                    => category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information, true)

                });

            optionBuilder.UseSqlServer("Server=localhost;Database=SoftUni;Integrated Security=true")
                .UseLoggerFactory(SqlCommandLoggerFactory);

            using (var context = new SoftUniContext(optionBuilder.Options))
            {
                //read
                var emp = context.Employees
                    .Include(e => e.Department)
                    .FirstOrDefault();

                // Update
                emp.FirstName = "Pesho";

                emp.LastName = "Ivanov";

                context.SaveChanges();
                               
                Console.WriteLine($"{emp.FirstName} {emp.LastName}, {emp.Department.Name}");

                // create
                var project = new Projects()
                {
                    Description = "Game",
                    Name = "Best game",
                    StartDate = DateTime.Now
                };


                context.Projects.Add(project);
                context.SaveChanges();

                ;

                // create
                var address = new Addresses()
                {
                    AddressText = "1 Vitosha Str",
                    Town = new Towns()
                    { Name = "Svoge" }
                };

                context.Addresses.Add(address);
                context.SaveChanges();
                ;

                //delete
                var town = context.Towns
                    .Include(t => t.Addresses)
                    .FirstOrDefault(t => t.Name == "Svoge");

                context.RemoveRange(town.Addresses);
                context.Towns.Remove(town);
                context.SaveChanges();
            }
        }
    }
}
