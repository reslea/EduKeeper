using EduKeeper.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduKeeper.Web.Models
{
    public class CourseCollectionModel
    {
        public int PageCount { get; set; }

        public IPagedList<Course> Courses { get; set; }
    }
}