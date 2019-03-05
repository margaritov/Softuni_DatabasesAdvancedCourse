namespace EFCodeFirst.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Reply
    {

        public int Id { get; set; }

        public int CommentId { get; set; }

        public int AuthorId { get; set; }

        [Required]
        [StringLength(250)]
        public string Content { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; }
    }
}
