using System;

namespace EduKeeper.Infrastructure.DTO
{
    public class FileDTO
    {
        public string Name { get; set; }

        public int PostId { get; set; }

        public Guid Identifier { get; set; }

        public string Path { get; set; }

        public string Extention { get { return System.IO.Path.GetExtension(Name); } }
    }
}
