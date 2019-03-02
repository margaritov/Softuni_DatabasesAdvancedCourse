using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDemoDBFirst.Data
{
    public partial class Addresses
    {
        public Addresses()
        {
            Employees = new HashSet<Employees>();
        }

        [Key]
        [Column("AddressID")]
        public int AddressId { get; set; }
        [Required]
        [StringLength(100)]
        public string AddressText { get; set; }
        [Column("TownID")]
        public int? TownId { get; set; }

        [ForeignKey("TownId")]
        [InverseProperty("Addresses")]
        public virtual Towns Town { get; set; }
        [InverseProperty("Address")]
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
