using System;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Entities
{
    public class Error : BaseEntity
    {
        [StringLength(50), Required]
        public string RedirectActionName { get; set; }

        [StringLength(50), Required]
        public string ErrorDescription { get; set; }

        [StringLength(50)]
        public string ErrorActionName { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
