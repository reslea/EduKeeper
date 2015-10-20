using System.ComponentModel.DataAnnotations;
using System.Web;

namespace EduKeeper.Web.Models
{
    public class UpdateUserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        public string LastName { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public bool Sex { get; set; }

        public HttpPostedFileBase PictureToUpdate { get; set; } 
    }
}