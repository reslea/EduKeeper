using EduKeeper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduKeeper.Infrastructure.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        public int CourseId { get; set; }

        public string Message { get; set; }

        public bool IsHasMore { get; set; }
        public DateTime DateWritten { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public List<File> Files { get; set; }
    }
}
