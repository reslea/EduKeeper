using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Entities
{
    public class Error
    {
        public int Id { get; set; }

        [StringLength(50), Required]
        public string RedirectActionName { get; set; }

        [StringLength(50), Required]
        public string ErrorDescription { get; set; }

        [StringLength(50)]
        public string ErrorActionName { get; set; }

        public virtual User User { get; set; }
    }
}
