using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Entities
{
    public class Post
    {

        public int Id { get; set; }

        public string Message { get; set; }

        [Required]
        public User Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }
}
