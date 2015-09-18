using EduKeeper.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduKeeper.Web.Models
{
    public class LeftMenuModel
    {
        public UserModel User { get; set; }

        public List<CourseDTO> Courses { get; set; }
    }
}