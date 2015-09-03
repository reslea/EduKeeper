using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduKeeper.Web.Models
{
    public class UserModel : LoginModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length 50 characters!")]
        public string LastName { get; set; }

        public int Age { get; set; }

        [Display(Name="TESTNAME")]
        public bool Sex { get; set; }

        public int GroupId { get; set; }

        public HttpPostedFileBase PictureToUpdate { get; set; } 
    }
}