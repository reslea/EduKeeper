using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduKeeper.Infrastructure.DTO
{
    public class FileDTO
    {
        public string Name { get; set; }

        public int PostId { get; set; }

        public Guid Identifier { get; set; }

        public string Path { get; set; }

        public string Extention { get; set; }
    }
}
