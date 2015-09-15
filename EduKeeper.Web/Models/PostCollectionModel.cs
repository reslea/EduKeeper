using EduKeeper.Infrastructure.DTO;
using PagedList;

namespace EduKeeper.Web.Models
{
    public class PostCollectionModel
    {
        public string CourseTitle { get; set; }

        public int CourseId { get; set; }

        public IPagedList<PostDTO> Posts { get; set; }
    }
}