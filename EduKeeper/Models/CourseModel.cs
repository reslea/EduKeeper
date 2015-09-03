using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduKeeper.Models
{
    public class CourseModel
    {
        public int Id { get; set; }

        public UserModel Owner { get; set; }

        [StringLength(100), Required]
        public string Title { get; set; }

        [StringLength(300), Required]
        public string Description { get; set; }
    }
}