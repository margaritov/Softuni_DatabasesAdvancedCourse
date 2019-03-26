namespace MyApp.Core.Commands
{
    using AutoMapper;
    using MyApp.Data;
    using System;
    using System.Collections.Generic;
    using System.Text;



    public class SetManagerCommand : ICommand
    {
        private readonly MyAppContext context;

        private readonly Mapper mapper;

        public SetManagerCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {

            var employeeId = int.Parse(inputArgs[0]);

            var managerId = int.Parse(inputArgs[1]);

            var employee = this.context.Employees.Find(employeeId);

            var manager = this.context.Employees.Find(managerId);

            employee.Manager = manager;

            context.SaveChanges();
            return "Manager set!"
        ;
        }
    }
}
