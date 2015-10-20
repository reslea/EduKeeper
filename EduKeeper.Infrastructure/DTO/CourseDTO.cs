using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Infrastructure.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }

        [StringLength(100), Required]
        public string Title { get; set; }

        [StringLength(300), Required]
        public string Description { get; set; }

        public bool IsUserJoined { get; set; }
    }
}
