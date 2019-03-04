namespace SoftUni.Models
{
    using System;
    using System.Collections.Generic;

    public partial class EmployeeProject
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public  Employee Employee { get; set; }

        public  Project Project { get; set; }
    }
}
