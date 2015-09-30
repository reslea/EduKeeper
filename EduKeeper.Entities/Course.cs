using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduKeeper.Entities
{
    public class Course : BaseEntity
    {
        public Course()
        {
            this.Users = new HashSet<User>();
        }

        [StringLength(100), Required]
        public string Title { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        public int? OwnerId { get; set; }

        public virtual ICollection<Post> Messages { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<User> MainCourses { get; set; }

        [StringLength(300), Required]
        public string Description { get; set; }

    }
}
