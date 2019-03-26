
namespace MyApp.Core.Commands
{
    using AutoMapper;
    using MyApp.Data;
    using MyApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public class EmployeePersonalInfoCommand : ICommand
    {
        private MyAppContext context;

        private Mapper mapper;

        public EmployeePersonalInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.mapper = mapper;

            this.context = context;

        }
        public string Execute(string[] inputArgs)
        {

            int targetEmployeeId;

            try
            {
                targetEmployeeId = int.Parse(inputArgs[0]);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Id must be a number!");
            }

            Employee employee = context.Employees
                .Where(e => e.Id == targetEmployeeId)
                .FirstOrDefault();

           

            string result = $"ID: {employee.Id} - " +
                $"{employee.FirstName} {employee.LastName} -" +
                $" ${employee.Salary:F2}"+Environment.NewLine+
                $"Birthday: {employee.Birthday}" + Environment.NewLine +
                $"Address: {employee.Address}";

            return result;
        }
    }
}