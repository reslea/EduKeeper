using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduKeeper.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(300, MinimumLength=8, ErrorMessage = "Should contains at least 8 characters!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}