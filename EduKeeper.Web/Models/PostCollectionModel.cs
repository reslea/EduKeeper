using EduKeeper.Entities;
using PagedList;

namespace EduKeeper.Web.Models
{
    public class PostCollectionModel
    {
        public int PageCount { get; set; }

        public IPagedList<Post> Posts { get; set; }
    }
}