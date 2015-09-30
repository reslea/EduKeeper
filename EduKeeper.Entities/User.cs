using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduKeeper.Entities
{
    public class User : BaseEntity
    {
        [StringLength(50), Required]
        public string FirstName { get; set; }

        [StringLength(50), Required]
        public string LastName { get; set; }

        [StringLength(100), Required]
        public string Email { get; set; }

        public int? Age { get; set; }

        public bool? Sex { get; set; }

        [ForeignKey("GroupId")]
        public virtual Course Group { get; set; }

        public int? GroupId { get; set; }

        public virtual ICollection<Course> OwnedGroups { get; set; }

        [StringLength(100), Required]
        public string Password { get; set; }

        public DateTime RegDate { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public DateTime LastVisited { get; set; }

        public string VisitedCourses { get; set; }
    }
}
