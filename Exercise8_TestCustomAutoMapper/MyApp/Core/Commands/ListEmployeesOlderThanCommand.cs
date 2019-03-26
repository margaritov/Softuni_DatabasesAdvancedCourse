namespace MyApp.Core.Commands
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using MyApp.Core.ViewModels;
    using MyApp.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        private MyAppContext context;

        private Mapper mapper;

        public ListEmployeesOlderThanCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            int age = int.Parse(inputArgs[0]);

            var employees = context.Employees
                .Include(e => e.Manager)
                .Where(e => e.Birthday.Value.Year < DateTime.Now.Year - age)
                .ToList();

            var employeeList = this.mapper.CreateMappedObject<EmployeeListDto>(employees);
            ;
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }
    }
}
