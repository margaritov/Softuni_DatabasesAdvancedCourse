using AutoMapper;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class EmployeeInfoCommand : ICommand
    {
        private MyAppContext context;

        private Mapper mapper;

        public EmployeeInfoCommand(MyAppContext context, Mapper mapper)
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

            var employeeDto = this.mapper
                .CreateMappedObject<EmployeeDto>(employee);

            string result = $"ID: {employeeDto.Id} - " +
                $"{employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:F2}";

            return result;
        }
    }
}
