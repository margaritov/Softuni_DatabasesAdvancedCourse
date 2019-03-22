namespace BillsPaymentSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

public class User
    {
        public User()
        {
            this.PaymentMethods = new HashSet<PaymentMethod>();
        }
        public int UserId { get; set; }

        [Required]
        [RegularExpression(@"^\S+@\S+$")]
        public string Email { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MinLength(6), MaxLength(20)]
        public string Password { get; set; }

        ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
