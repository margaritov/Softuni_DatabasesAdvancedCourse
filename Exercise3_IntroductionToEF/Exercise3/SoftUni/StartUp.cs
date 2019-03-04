namespace SoftUni
{
    using SoftUni.Data;
    using SoftUni.Models;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {

                string result;
                // Task 3. Employees Full Information
                 result = GetEmployeesFullInformation(context);
                 Console.WriteLine(result);

                // Task 4. Employees with salary over 50,000
                 result = GetEmployeesWithSalaryOver50000(context);
                 Console.WriteLine(result);


                // Task 5. Employees from Research and Development
                 result =  GetEmployeesFromResearchAndDevelopment(context);
                 Console.WriteLine(result);

                // Task 6. Adding a New Address and Updating Employee
                 result= AddNewAddressToEmployee(context);
                 Console.WriteLine(result);

                // Task 7. Employees and Projects
                result = GetEmployeesInPeriod(context);
                Console.WriteLine(result);

                //Task 8. Addresses by Town
                result = GetAddressesByTown(context);
                Console.WriteLine(result);

                // Task 9. Employee 147
                 result = GetEmployee147(context);
                 Console.WriteLine(result);

                // Task 10. Employee with More Than 5 Employees
                result = GetDepartmentsWithMoreThan5Employees(context);
                Console.WriteLine(result);

                // Task 11. Find Latest 10 Projects
                result = GetLatestProjects(context);
                Console.WriteLine(result);

                // Task 12. Increase Salaries
                 result = IncreaseSalaries(context);
                 Console.WriteLine(result);

                // Task 13. Find Employees by First Name Starting with "Sa"
                 result = GetEmployeesByFirstNameStartingWithSa(context);
                 Console.WriteLine(result);

                //Task 14. Delete Project by Id
                result = DeleteProjectById(context);
                Console.WriteLine(result);

                //Task 15. Remove Town
                result = RemoveTown(context);
                Console.WriteLine(result);
                
            }
        }
        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns
                .First(t => t.Name == "Seattle");
            
            var addresses = context.Addresses
                .Where(a => a.Town == town)
                .ToList();
            
            var employees = context.Employees
                .Where(e => addresses.Contains(e.Address))
                .ToList();

            employees.ForEach(e => e.Address = null);

            context.Addresses.RemoveRange(addresses);

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{addresses.Count} addresses in {town.Name} were deleted";
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);


            context.EmployeesProjects.RemoveRange(
                context.EmployeesProjects.Where(ep => ep.ProjectId == 2));

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name
                })
                .Take(10)
                .ToList();
            ;

            StringBuilder sb = new StringBuilder();

            projects.ForEach(p => sb.AppendLine($"{p.Name}"));

            return sb.ToString().TrimEnd();
        }


        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departments = {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };
            var employees = context.Employees
                .Where(e => departments.Contains(e.Department.Name))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                e.Salary *= 1.12M;
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }
            context.SaveChanges();
            return sb.ToString().TrimEnd();


        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            projects.ForEach(p => sb.AppendLine($"{p.Name}\r\n{p.Description}\r\n{p.StartDate}"));

            return sb.ToString().TrimEnd();
        }


        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerFirstName}" +
                    $" {department.ManagerLastName}");
                foreach (var employee in department.Employees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString();

        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(p => new
                        {
                            ProjectName = p.Project.Name
                        })
                        .OrderBy(p => p.ProjectName)
                        .ToList()
                })
                .ToList()
                .FirstOrDefault();
            ;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.Projects)
            {
                sb.AppendLine($"{project.ProjectName}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    EmployeesCount = x.Employees.Count
                })
                .Take(10)
                .ToList()
                ;



            StringBuilder sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
            }
            return sb.ToString().TrimEnd();
        }


        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(p => p.EmployeesProjects.Any(s => s.Project.StartDate.Year >= 2001 &&
               s.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFullName = x.FirstName + " " + x.LastName,
                    ManagerFullName = x.Manager.FirstName + " " + x.Manager.LastName,
                    Projects = x.EmployeesProjects
                        .Select(p => new
                        {
                            ProjectName = p.Project.Name,
                            StartDate = p.Project.StartDate,
                            EndDate = p.Project.EndDate
                        })
                        .ToList()
                })
                .Take(10)
                .ToList()
            ;
            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.EndDate == null ?
                       "not finished" : project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt");
                    sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }
            ;
            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(address);


            var nakov = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov")
                .Address = address;
            context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            var employeeAddresses = context.Employees
                .Select(x => new
                {
                    x.AddressId,
                    x.Address.AddressText
                })
                .OrderByDescending(a => a.AddressId)
                .Take(10)
                .ToList();

            foreach (var employeeAddress in employeeAddresses)
            {
                sb.AppendLine($"{employeeAddress.AddressText}");
            }

            return sb.ToString().TrimEnd();


        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
               .OrderBy(e => e.Salary)
               .ThenByDescending(e => e.FirstName)
               .ToList();
            ;
            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}");
            }

            return sb.ToString();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
               .Select(e => new
               {
                   e.FirstName,
                   e.Salary
               })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }
            return sb.ToString();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                })
                .OrderBy(x => x.EmployeeId)
                .ToList();
            ;

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
            return sb.ToString();
        }
    }
}
