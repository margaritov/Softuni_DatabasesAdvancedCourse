using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDemoDBFirst.Data
{
    public partial class Employees
    {
        public Employees()
        {
            Departments = new HashSet<Departments>();
            EmployeesProjects = new HashSet<EmployeesProjects>();
            InverseManager = new HashSet<Employees>();
        }

        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(50)]
        public string JobTitle { get; set; }
        [Column("DepartmentID")]
        public int DepartmentId { get; set; }
        [Column("ManagerID")]
        public int? ManagerId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime HireDate { get; set; }
        [Column(TypeName = "money")]
        public decimal Salary { get; set; }
        [Column("AddressID")]
        public int? AddressId { get; set; }

        [ForeignKey("AddressId")]
        [InverseProperty("Employees")]
        public virtual Addresses Address { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("Employees")]
        public virtual Departments Department { get; set; }
        [ForeignKey("ManagerId")]
        [InverseProperty("InverseManager")]
        public virtual Employees Manager { get; set; }
        [InverseProperty("Manager")]
        public virtual ICollection<Departments> Departments { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeesProjects> EmployeesProjects { get; set; }
        [InverseProperty("Manager")]
        public virtual ICollection<Employees> InverseManager { get; set; }
    }
}
