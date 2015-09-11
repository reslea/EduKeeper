using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduKeeper.Entities
{
    public class Post
    {

        public int Id { get; set; }

        public string Message { get; set; }

        public int? AuthorId { get; set; }

        public int CourseId { get; set; }

        public User Author { get; set; }

        public Course Course { get; set; }

        public DateTime DateWritten { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }
}
