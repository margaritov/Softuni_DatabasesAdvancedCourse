namespace EFCodeFirst.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Post
    /// </summary>

    [Table("Posts")]
    public class Post
    {
        /// <summary>
        /// Post identifier
        /// </summary>
        [Key]
        [Column("Id")]
        public int PostId { get; set; }

        /// <summary>
        /// Post author
        /// </summary>
        [Required]
        public int AuthorId { get; set; }

        /// <summary>
        /// Post title
        /// </summary>
        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Title { get; set; }

        /// <summary>
        /// Post content
        /// </summary>
        [Required]
        [StringLength(5000, MinimumLength = 10)]
        public string Content { get; set; }

        /// <summary>
        /// Post Author
        /// </summary>

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

    }
}
