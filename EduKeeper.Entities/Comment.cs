using System;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Entities
{
    public class Comment : BaseEntity
    {
        [StringLength(4000), Required]
        public string Message { get; set; }

        public int? AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public DateTime DateWritten { get; set; }

        //public virtual ICollection<File> Files { get; set; }
    }
}
