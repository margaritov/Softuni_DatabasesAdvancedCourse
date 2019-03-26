using AutoMapper;
using MyApp.Data;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private MyAppContext context;

        private Mapper mapper;

        public SetBirthdayCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            //int targetEmployeeId = int.TryParse(inputArgs[0]);
            int targetEmployeeId;

            try
            {
                targetEmployeeId = int.Parse(inputArgs[0]);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Id must be a number!");
            }

            DateTime birthday;
            try
            {
                birthday = DateTime.ParseExact(inputArgs[1], "dd-MM-yyyy",
                CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid birthday format!");
            }

           


            Employee employee = context.Employees
                .Where(e => e.Id == targetEmployeeId)
                .FirstOrDefault();

            if (employee == null)
            {
                throw new InvalidOperationException($"Employee with ID " +
                    $"{targetEmployeeId} not found!");
            }

            employee.Birthday = birthday;

            context.SaveChanges();

            return $"Employee ID {targetEmployeeId} - birthday updated";

        }
    }
}
