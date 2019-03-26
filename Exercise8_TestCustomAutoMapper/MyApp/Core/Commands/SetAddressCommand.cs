using AutoMapper;
using MyApp.Data;
using MyApp.Models;
using System;
using System.Linq;

namespace MyApp.Core.Commands
{
    public class SetAddressCommand : ICommand
    {
        private MyAppContext context;

        private Mapper mapper;

        public SetAddressCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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

            string address = String.Join(" ", inputArgs.Skip(1));

            Employee employee = context.Employees
                .Where(e => e.Id == targetEmployeeId)
                .FirstOrDefault();

            employee.Address = address;
            context.SaveChanges();

            string result = $"Employee ID {targetEmployeeId} address set to: {address}";

            return result;

        }
    }
}