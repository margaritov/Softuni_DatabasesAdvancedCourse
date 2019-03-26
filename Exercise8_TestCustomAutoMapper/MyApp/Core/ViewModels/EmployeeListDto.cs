using MyApp.Models;

namespace MyApp.Core.ViewModels
{
    public class EmployeeListDto
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }
    }
}
