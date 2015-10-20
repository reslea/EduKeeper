using EduKeeper.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Infrastructure.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        [StringLength(4000, MinimumLength = 1)]
        public string Message { get; set; }

        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        [Required]
        public int PostId { get; set; }

        public DateTime DateWritten { get; set; }

        public List<File> Files { get; set; }
    }
}
