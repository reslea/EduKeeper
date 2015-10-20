using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Web.Models
{
    public class LoginModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(300, MinimumLength=8, ErrorMessage = "Should contains at least 8 characters!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}