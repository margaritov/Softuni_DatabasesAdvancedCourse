
namespace SoftJail.Data.Models
{
    using SoftJail.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Officer
    {

        public Officer()
        {
            this.OfficerPrisoners = new List<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string FullName { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        public Position Position { get; set; }

        public Weapon Weapon { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }

    }
}
