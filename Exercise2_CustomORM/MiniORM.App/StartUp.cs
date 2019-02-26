namespace MiniORM.App
{
    using MiniORM.App.Data;
    using MiniORM.App.Data.Entities;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var connectionString = "Server=localhost;Database=MiniORM;Integrated security=True";

            var context = new SoftUniDbContext(connectionString);
            ;
            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true,
            });

            var employee = context.Employees.Last();
            employee.FirstName = "Modified";
            context.SaveChanges();
        }
    }
}
