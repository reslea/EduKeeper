using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Web.Models
{
    public class UserModel : LoginModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        public string LastName { get; set; }

        public int Age { get; set; }

        public bool Sex { get; set; }
    }
}