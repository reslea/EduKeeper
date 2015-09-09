using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduKeeper.Infrastructure
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        public int CourseId { get; set; }

        public string Message { get; set; }
    }
}
