using System;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Entities
{
    public class Event : BaseEntity
    {
        [StringLength(50), Required]
        public string Title { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        //public DateTime EndTime { get; set; }

        public Course Course { get; set; }
    }
}
