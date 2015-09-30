using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduKeeper.Entities
{
    public class Post : BaseEntity
    {
        public string Message { get; set; }

        public int? AuthorId { get; set; }

        public int CourseId { get; set; }

        public virtual User Author { get; set; }

        public virtual Course Course { get; set; }

        public DateTime DateWritten { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }
}
