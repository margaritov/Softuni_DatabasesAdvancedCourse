

namespace SoftJail.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Prisoner
    {

        public Prisoner()
        {
            this.Mails = new List<Mail>();

            this.PrisonerOfficers = new List<OfficerPrisoner>();
        }


        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }

        [RegularExpression(@"^The [A-Z][a-z]+$")]
        public string Nickname { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public DateTime IncarcerationDate { get; set; }

        //not required value type - make it nullable
        public DateTime? ReleaseDate { get; set; }

        //TODO: NB may be decimal, not nullable
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }
                
        public int? CellId { get; set; }
        public Cell Cell { get; set; }

        public ICollection<Mail> Mails { get; set; }

        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; }

    }
}