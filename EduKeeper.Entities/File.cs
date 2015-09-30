using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

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
