using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Entities
{
    public class Post : BaseEntity
    {
        [StringLength(4000), Required]
        public string Message { get; set; }

        public int AuthorId { get; set; }

        public int CourseId { get; set; }

        public virtual User Author { get; set; }

        public virtual Course Course { get; set; }

        public DateTime DateWritten { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }
}
