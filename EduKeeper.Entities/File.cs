using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EduKeeper.Entities
{
    public class File
    {
        public int Id { get; set; }

        [StringLength(100), Required]
        public string Link { get; set; } //it will be server link, like "~/UsersContent/asd.jpg"
    }
}
