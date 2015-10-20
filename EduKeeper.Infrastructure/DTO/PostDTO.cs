using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Infrastructure.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [StringLength(4000, MinimumLength=1)]
        public string Message { get; set; }

        public bool IsHasMoreComments { get; set; }

        public DateTime DateWritten { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public List<FileDTO> Files { get; set; }
    }
}
