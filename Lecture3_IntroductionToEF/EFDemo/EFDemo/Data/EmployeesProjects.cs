using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDemoDBFirst.Data
{
    public partial class EmployeesProjects
    {
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Column("ProjectID")]
        public int ProjectId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeesProjects")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("EmployeesProjects")]
        public virtual Projects Project { get; set; }
    }
}
