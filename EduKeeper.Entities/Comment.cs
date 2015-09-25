using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduKeeper.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public int? AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ICollection<Document> Files { get; set; }

        public DateTime DateWritten { get; set; }
    }
}
