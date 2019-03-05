namespace EFCodeFirst.Infrastructure.Data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string Email { get; set; }

        public HashSet<Post> Posts { get; set; }

        public HashSet<Comment> Comments { get; set; }

        public HashSet<Reply> Replies { get; set; }

        

        public User()
        {
            this.Posts = new HashSet<Post>();

            this.Comments = new HashSet<Comment>();

            this.Replies = new HashSet<Reply>();

        }
    }
}
