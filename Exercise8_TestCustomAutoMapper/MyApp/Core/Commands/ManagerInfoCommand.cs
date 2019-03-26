namespace MyApp.Core.Commands
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using MyApp.Core.ViewModels;
    using MyApp.Data;
    using System.Linq;
    using System.Text;

    public class ManagerInfoCommand : ICommand
    {
        private MyAppContext context;

        private Mapper mapper;

        public ManagerInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            int managerId = int.Parse(inputArgs[0]);

            var manager = this.context.Employees
                .Include(m => m.ManagedEmployees)
                .FirstOrDefault(x => x.Id == managerId);

            var managerDto = this.mapper.CreateMappedObject<ManagerDto>(manager);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: " +
                $"{managerDto.ManagedEmployees.Count}");
            foreach (var employee in managerDto.ManagedEmployees)
            {
                sb.AppendLine($"   - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}");
            }

            return sb.ToString();
        }
    }
}
