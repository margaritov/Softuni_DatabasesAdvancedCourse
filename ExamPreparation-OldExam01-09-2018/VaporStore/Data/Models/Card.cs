
namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class Card
    {
        public Card()
        {
            this.Purchases = new List<Purchase>();
        }

        [Key]
        public int Id { get; set; }

        //todo regex
        [RegularExpression(@"^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")]
        public string Number { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Cvc { get; set; }

        public CardType Type { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}
