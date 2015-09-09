using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using PagedList;
using System.Collections.Generic;

namespace EduKeeper.Web.Models
{
    public class PostCollectionModel
    {
        public string CourseTitle { get; set; }

        public int CourseId { get; set; }

        public IPagedList<PostDTO> Posts { get; set; }
    }
}