using EduKeeper.Infrastructure.DTO;
using System.Collections.Generic;

namespace EduKeeper.Web.Models
{
    public class LeftMenuModel
    {
        public UserModel User { get; set; }

        public List<CourseDTO> Courses { get; set; }
    }
}