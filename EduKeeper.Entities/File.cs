using System;
using System.ComponentModel.DataAnnotations;

namespace EduKeeper.Entities
{
    public class File : BaseEntity
    {
        public Guid Identifier { get; set; }

        [StringLength(255), Required]
        public string Path { get; set; }

        [StringLength(100), Required]
        public string Name { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
